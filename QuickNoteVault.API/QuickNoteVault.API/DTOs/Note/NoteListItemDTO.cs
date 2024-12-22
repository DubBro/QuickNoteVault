﻿using Newtonsoft.Json.Linq;

namespace QuickNoteVault.API.DTOs.Note;

public class NoteListItemDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public JArray Content { get; set; } = new JArray();
    public DateTime ModifiedAt { get; set; }
    public int UserId { get; set; }
}
