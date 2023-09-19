using Core.Models;
using SendGrid.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class SendGridExtensions
    {
        public static IServiceCollection AddSendGrindServices(this IServiceCollection services, IConfiguration config)
        {

            services.Configure<SendGridSettings>(config.GetSection("SendGridSettings"));
            services.AddSendGrid(options =>
            {
                options.ApiKey = config.GetSection("SendGridSettings")
                    .GetValue<string>("ApiKey");
            });


            return services;
        }
    }
}