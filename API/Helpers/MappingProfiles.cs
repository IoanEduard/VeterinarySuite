
using API.DTO;
using AutoMapper;
using Core.Entities.Identity;
using Core.Models;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserCreationDto, TenantUser>();
            CreateMap<SubscriptionPlanDto, SubscriptionPlan>().ReverseMap();
        }
    }
}