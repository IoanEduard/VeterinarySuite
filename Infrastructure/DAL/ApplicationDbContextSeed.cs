using System.Text.Json;
using Core.Entities;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DAL
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Patients.Any())
                {
                    var patientsData = File.ReadAllText("../Infrastructure/SeedData/patients.json");

                    var patients = JsonSerializer.Deserialize<List<Patient>>(patientsData);

                    foreach (var item in patients)
                    {
                        context.Patients.Add(item);
                    }

                    await context.SaveChangesAsync();
                }
            }

            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
                logger.LogError(ex.Message);
            }
        }

        public static async Task SeedSubscriptionPlansAsync(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.SubscriptionPlans.Any())
                {
                    var subscriptionPlans = File.ReadAllText("../Infrastructure/SeedData/subscriptionPlans.json");

                    var subscriptionPlansDeserialized = JsonSerializer.Deserialize<List<SubscriptionPlan>>(subscriptionPlans);

                    foreach (var item in subscriptionPlansDeserialized)
                    {
                        context.SubscriptionPlans.Add(item);
                    }

                    await context.SaveChangesAsync();
                }
            }

            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
                logger.LogError(ex.Message);
            }
        }
    }
}