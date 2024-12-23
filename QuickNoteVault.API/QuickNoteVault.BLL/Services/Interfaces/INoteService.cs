using QuickNoteVault.BLL.Models;

namespace QuickNoteVault.BLL.Services.Interfaces;

public interface INoteService
{
    Task<ICollection<NoteModel>> GetAllByUserIdAsync(int userId);
    Task<NoteModel> GetByIdAsync(int id);
    Task<int> AddAsync(NoteModel noteModel);
    Task<int> UpdateAsync(NoteModel noteModel);
    Task<int> DeleteByIdAsync(int id);
}
