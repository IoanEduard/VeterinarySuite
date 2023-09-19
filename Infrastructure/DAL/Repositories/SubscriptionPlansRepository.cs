using Core.Interfaces;
using Core.Models;

namespace Infrastructure.DAL.Repositories
{
    public class SubscriptionPlansRepository : GenericRepository<SubscriptionPlan>, ISubscriptionPlansRepository
    {
        public SubscriptionPlansRepository(ApplicationDbContext context) 
            : base(context)
        {
        }
    }
}