using API.DTO;
using API.Errors;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Models.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Owner")]
    public class OwnerController : BaseApiController
    {
        private readonly UserManager<TenantUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IEmailTemplateGeneratorObject _emailTemplateGenerator;
        private readonly IMapper _mapper;

        public OwnerController(UserManager<TenantUser> userManager,
                RoleManager<IdentityRole> roleManager,
                IEmailSenderService emailSenderService,
                IEmailTemplateGeneratorObject emailTemplateGenerator,
                IMapper mapper)
        {
            _emailTemplateGenerator = emailTemplateGenerator;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSenderService = emailSenderService;
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser(UserCreationDto userForCreationDto)
        {
            if (await _userManager.FindByEmailAsync(userForCreationDto.Email) != null)
                return new BadRequestObjectResult(new ApiResponse(409));

            var user = _mapper.Map<UserCreationDto, TenantUser>(userForCreationDto);
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));
            else
            {
                await AddRolesToUser(userForCreationDto.Roles, user);
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                if (!string.IsNullOrEmpty(token))
                {
                    var emailTemplateParams = _emailTemplateGenerator.CreateEmailTemplateObject(
                        user.Email,
                        "Account Confirmation",
                        token,
                        EmailTemplateSelectorEnum.AccountActivation);

                    await _emailSenderService.SendEmailAsync(emailTemplateParams);
                }
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost("assignRolesToUser")]
        public async Task<IActionResult> AssignRolesToUser(IList<string> rolesSubmmitedByUser, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest(new ApiResponse(400));

            await AddRolesToUser(rolesSubmmitedByUser, user);

            return Ok();
        }

        [HttpPost("lockUser")]
        public async Task<IActionResult> LockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest(new ApiResponse(400));

            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

            return Ok();
        }

        [HttpPost("unlocUser")]
        public async Task<IActionResult> UnlockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest(new ApiResponse(400));

            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddMinutes(-1));

            return Ok();
        }

        private async Task AddRolesToUser(IList<string> rolesSubmmitedByUser, TenantUser user)
        {
            var roles = new List<string>();
            foreach (var roleSubmitedByOwner in rolesSubmmitedByUser)
            {
                var role = await _roleManager.FindByIdAsync(roleSubmitedByOwner);
                if (role != null)
                    roles.Add(role.Name);
            }

            await _userManager.AddToRolesAsync(user, roles);
        }
    }
}