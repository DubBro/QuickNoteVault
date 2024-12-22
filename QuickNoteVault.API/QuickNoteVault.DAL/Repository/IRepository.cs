using System.Linq.Expressions;

namespace QuickNoteVault.DAL.Repository;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity> AddAsync(TEntity entity);
    TEntity Update(TEntity entity);
    TEntity Delete(TEntity entity);
    Task<TEntity?> FirstOrDefaultAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);
}
