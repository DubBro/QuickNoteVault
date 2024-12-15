using Microsoft.EntityFrameworkCore;
using QuickNoteVault.DAL.Repository;

namespace QuickNoteVault.DAL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        private bool _disposed = false;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public IRepository<T> GetRepository<T>()
            where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>).MakeGenericType(type);
                var repositoryInstance = Activator.CreateInstance(repositoryType, _context);

                if (repositoryInstance != null)
                {
                    _repositories[type] = repositoryInstance;
                }
                else
                {
                    throw new InvalidOperationException($"Failed to create repository for type {type.FullName}");
                }
            }

            return (IRepository<T>)_repositories[type];
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
    }
}
