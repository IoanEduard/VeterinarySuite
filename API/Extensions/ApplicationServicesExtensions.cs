using API.Helpers;
using Core.Interfaces;
using Core.Models;
using Infrastructure.DAL.Repositories;
using Infrastructure.Services;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection ApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IEmailTemplateGeneratorObject, EmailTemplateGeneratorObject>();
            services.Configure<DevelopmentBackendPaths>(config.GetSection("DevelopmentBackendPaths"));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ISubscriptionPlansRepository, SubscriptionPlansRepository>(); 

            return services;
        }
    }
}