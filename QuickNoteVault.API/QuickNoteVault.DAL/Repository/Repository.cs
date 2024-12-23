using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace QuickNoteVault.DAL.Repository;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext context)
    {
        _dbSet = context.Set<TEntity>();
    }

    public async Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        var entityEntry = await _dbSet.AddAsync(entity);
        return entityEntry.Entity;
    }

    public TEntity Update(TEntity entity)
    {
        var entityEntry = _dbSet.Update(entity);
        return entityEntry.Entity;
    }

    public TEntity Delete(TEntity entity)
    {
        var entityEntry = _dbSet.Remove(entity);
        return entityEntry.Entity;
    }

    public async Task<TEntity?> FirstOrDefaultAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
    }
}
