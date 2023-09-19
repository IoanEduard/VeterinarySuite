using Core.Entities.Identity;
using Infrastructure.DAL;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class SeedExtension
    {
        public static async Task<WebApplication> SeedServiceExtension(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    await context.Database.MigrateAsync();
                    await ApplicationDbContextSeed.SeedAsync(context, loggerFactory);
                    await ApplicationDbContextSeed.SeedSubscriptionPlansAsync(context, loggerFactory);

                    var userManager = services.GetRequiredService<UserManager<TenantUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                    await identityContext.Database.MigrateAsync();
                    await AppIdentityRolesSeed.SeedRolesAsync(roleManager);
                    await AppIdentityDbContextSeed.SeedUserAsync(userManager);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "error durring migration");
                }
            }

            return app;
        }
    }
}