namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;
        ISubscriptionPlansRepository SubscriptionPlansRepository();
        Task<int> Complete();
    }
}