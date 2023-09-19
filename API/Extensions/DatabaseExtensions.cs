using Infrastructure.DAL;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection DatabaseServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddDbContext<AppIdentityDbContext>(x => x.UseSqlServer(config.GetConnectionString("IdentityConnection")));

            return services;
        }
    }
}