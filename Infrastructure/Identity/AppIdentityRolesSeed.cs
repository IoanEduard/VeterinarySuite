using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityRolesSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {

            var roles = new[] { "Owner", "Admin", "Employer" };

            foreach (var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role)) {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}