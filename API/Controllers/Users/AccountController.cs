
using System.Text;
using API.DTO;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Models.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<TenantUser> _userManager;
        private readonly SignInManager<TenantUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IEmailTemplateGeneratorObject _emailTemplateGenerator;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<TenantUser> userManager,
                                    SignInManager<TenantUser> signInManager,
                                    ITokenService tokenService,
                                    IMapper mapper,
                                    IEmailSenderService emailSenderService,
                                    IEmailTemplateGeneratorObject emailTemplateGenerator)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _emailSenderService = emailSenderService;
            _emailTemplateGenerator = emailTemplateGenerator;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiException(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiException(401));

            var userRoles = await _userManager.GetRolesAsync(user);
            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user, userRoles),
                DisplayName = user.DisplayName
            };
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiResponse(409));
            }

            var newUser = new TenantUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            else
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                if (!string.IsNullOrEmpty(token))
                {
                    var emailTemplateParams = _emailTemplateGenerator.CreateEmailTemplateObject(
                        newUser.Email,
                        "Account Confirmation",
                        token,
                        EmailTemplateSelectorEnum.ConfirmEmail
                    );

                    await _emailSenderService.SendEmailAsync(emailTemplateParams);
                }
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new ApiResponse(400));

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok("Email confirmed successfully");
        }

        [HttpPost("recoverPassword")]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordDto recoverPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(recoverPasswordDto.Email);
            if (user == null)
                return BadRequest(new ApiResponse(400));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                var emailTemplateParams = _emailTemplateGenerator.CreateEmailTemplateObject(
                    user.Email,
                    "Recover Password",
                    token,
                    EmailTemplateSelectorEnum.RecoverPassword
                );

                await _emailSenderService.SendEmailAsync(emailTemplateParams);
            }

            return NoContent();
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTo resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest(new ApiResponse(404));

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = new StringBuilder();
                foreach (var error in resetPassResult.Errors)
                    errors.Append($"{error.Code} - {error.Description}\n");

                return BadRequest(new ApiResponse(400, $"failed to update password\n {errors}"));
            }

            await _userManager.ConfirmEmailAsync(user, resetPasswordDto.Token);

            return Ok("Password reset");
        }

        [AllowAnonymous]
        [HttpPost("setPasswordToAccount")]
        public async Task<IActionResult> SetPasswordToAccount(ActivateAccountDto activateAccountDto)
        {
            var user = await _userManager.FindByEmailAsync(activateAccountDto.Email);
            if (user == null)
                return BadRequest(new ApiResponse(404));

            var resetPassResult = await _userManager.ResetPasswordAsync(user, activateAccountDto.ActivationLinkToken, activateAccountDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = new StringBuilder();
                foreach (var error in resetPassResult.Errors)
                    errors.Append($"{error.Code} - {error.Description}\n");

                return BadRequest(new ApiResponse(400, $"failed to update password\n {errors}"));
            }
            else
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddMinutes(-1));
            }

            return Ok("Password reset");
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindByClaimsPrincipalWithAddressAsync(HttpContext.User);

            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FindByClaimsPrincipalWithAddressAsync(HttpContext.User);

            var adr = _mapper.Map<AddressDto, Address>(address);

            user.Address = adr;
            user.Address.AppUserId = user.Id;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));

            return BadRequest("Update Failed");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailWithFromClaimsPrincipalAsync(HttpContext.User);
            var userRoles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user, userRoles),
                DisplayName = user.DisplayName,
            };
        }

        private async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}