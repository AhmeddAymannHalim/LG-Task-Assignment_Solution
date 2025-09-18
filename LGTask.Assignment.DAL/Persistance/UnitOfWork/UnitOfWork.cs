
using LGTask.Assignment.DAL.Persistance.Data;

namespace LGTask.Assignment.DAL.Persistance.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ITDepartmentDbContext _dbContext;

        public UnitOfWork(ITDepartmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> CompleteAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error saving changes to database", ex);
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
    }
}
