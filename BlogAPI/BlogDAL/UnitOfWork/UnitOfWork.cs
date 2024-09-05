using BlogDAL.IService;
using BlogDAL.Service;

namespace BlogDAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;

        private AppDbContext context;
        public UnitOfWork(AppDbContext _context)
        {
            context = _context;
        }
        public IGenericService<T> GetRepository<T>() where T : class
        {
            return new GenericService<T>(context);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
