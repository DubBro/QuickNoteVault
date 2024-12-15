using System.Linq.Expressions;

namespace QuickNoteVault.DAL.Repository
{
    public interface IRepository<T>
        where T : class
    {
        Task<ICollection<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteByIdAsync(int id);
        Task<ICollection<T>> FindByConditionAsync(Expression<Func<T, bool>> predicate);
    }
}
