using Newtonsoft.Json.Linq;

namespace QuickNoteVault.BLL.Models;

public class NoteModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public JArray Content { get; set; } = new JArray();
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public int UserId { get; set; }
}
