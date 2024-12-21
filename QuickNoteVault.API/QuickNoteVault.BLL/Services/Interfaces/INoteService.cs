using QuickNoteVault.BLL.Models;

namespace QuickNoteVault.BLL.Services.Interfaces;

public interface INoteService
{
    Task<ICollection<NoteModel>> GetAllByUserIdAsync(int userId);
    Task<NoteModel> GetByIdAsync(int id);
    Task AddAsync(NoteModel noteModel);
    Task UpdateAsync(NoteModel noteModel);
    Task DeleteByIdAsync(int id);
}
