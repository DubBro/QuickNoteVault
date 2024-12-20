using System.Linq.Expressions;

namespace QuickNoteVault.DAL.Repository
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task<ICollection<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        Task DeleteByIdAsync(int id);
        Task<ICollection<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
