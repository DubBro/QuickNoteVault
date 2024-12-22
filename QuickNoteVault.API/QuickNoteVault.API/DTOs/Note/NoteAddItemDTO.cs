using Newtonsoft.Json.Linq;

namespace QuickNoteVault.API.DTOs.Note;

public class NoteAddItemDTO
{
    public string Title { get; set; } = string.Empty;
    public JArray Content { get; set; } = new JArray();
    public int UserId { get; set; }
}
