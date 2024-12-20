using Microsoft.EntityFrameworkCore.Storage;
using QuickNoteVault.DAL.Repository;

namespace QuickNoteVault.DAL.UOW;

public interface IUnitOfWork : IDisposable
{
    Task SaveAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    IRepository<TEntity> GetRepository<TEntity>()
        where TEntity : class;
}
