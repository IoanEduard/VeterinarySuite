using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class TenantUser : IdentityUser
    {
        public BusinessInformation BusinessInformation {get;set;}
    }
}