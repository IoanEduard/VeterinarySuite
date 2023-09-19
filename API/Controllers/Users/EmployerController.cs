using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.RolesTesting
{
    [Authorize(Roles = "Owner, Employer")]
    public class EmployerController : BaseApiController
    {
        [HttpGet("test")]
        public string Test()
        {
            return "employer controller reached";
        }
    }
}