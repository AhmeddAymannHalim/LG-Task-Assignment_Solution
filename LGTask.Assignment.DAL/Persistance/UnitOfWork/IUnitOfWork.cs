namespace LGTask.Assignment.DAL.Persistance.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task<int> CompleteAsync();
    }
}
