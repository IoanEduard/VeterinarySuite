using API.DTO;
using API.Errors;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SubscriptionPlansController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubscriptionPlansController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("allSubscriptionPlans")]
        public async Task<ActionResult<SubscriptionPlanDto>> GetAllSubscriptionPlansAsync()
        {
            var plans = await _unitOfWork.SubscriptionPlansRepository().GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<SubscriptionPlan>, IEnumerable<SubscriptionPlanDto>>(plans));
        }

        [HttpGet("subscriptionPlan/{id}")]
        public async Task<ActionResult<SubscriptionPlanDto>> GetSubscriptionPlanByIdAsync(int id)
        {
            var plans = await _unitOfWork.SubscriptionPlansRepository().GetByIdAsync(id);

            return Ok(_mapper.Map<SubscriptionPlan, SubscriptionPlanDto>(plans));
        }

        [HttpPost("addSubscriptionPlan")]
        public async Task<ActionResult<SubscriptionPlanDto>> AddSubscriptionPlanAsync(SubscriptionPlanDto planDto)
        {
            var plan = _mapper.Map<SubscriptionPlanDto, SubscriptionPlan>(planDto);

            if (!await _unitOfWork.SubscriptionPlansRepository().AnyAsyncByName(p => p.Name == planDto.Name))
                await _unitOfWork.SubscriptionPlansRepository().AddAsync(plan);
            else return BadRequest(new ApiResponse(409));

            var planToReturn = _mapper.Map<SubscriptionPlan, SubscriptionPlanDto>(plan);

            return await _unitOfWork.Complete() > 0 ? Ok(planToReturn) : BadRequest(new ApiResponse(400));
        }

        [HttpPut("updateSubscriptionPlan")]
        public async Task<ActionResult<SubscriptionPlanDto>> UpdateSubscriptionPlanAsync(SubscriptionPlanDto planDto)
        {
            var planFromDb = await _unitOfWork.SubscriptionPlansRepository().GetByIdAsync(planDto.Id);

            if (planFromDb == null)
                return NotFound(new ApiResponse(404));

            planFromDb.Name = planDto.Name;
            planFromDb.Description = planDto.Description;
            planFromDb.Price = planDto.Price;
            planFromDb.IsActive = planDto.IsActive;
            planFromDb.Days = planDto.Days;

            _unitOfWork.SubscriptionPlansRepository().Update(planFromDb);

            var planToReturn = _mapper.Map<SubscriptionPlan, SubscriptionPlanDto>(planFromDb);

            return await _unitOfWork.Complete() > 0 ? Ok(planToReturn) : BadRequest(new ApiResponse(400));
        }

        [HttpDelete("deleteSubscriptionPlan/{id}")]
        public async Task<ActionResult<SubscriptionPlanDto>> DeleteSubscriptionPlanAsync(int id)
        {
            var planFromDb = await _unitOfWork.SubscriptionPlansRepository().GetByIdAsync(id);
            if (planFromDb == null)
                return NotFound(new ApiResponse(404));

            _unitOfWork.SubscriptionPlansRepository().Remove(planFromDb);

            return await _unitOfWork.Complete() > 0 ? NoContent() : BadRequest(new ApiResponse(400));
        }
    }
}