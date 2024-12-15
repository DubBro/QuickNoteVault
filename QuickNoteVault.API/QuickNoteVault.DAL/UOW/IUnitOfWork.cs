using QuickNoteVault.DAL.Repository;

namespace QuickNoteVault.DAL.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveAsync();
        IRepository<T> GetRepository<T>()
            where T : class;
    }
}
