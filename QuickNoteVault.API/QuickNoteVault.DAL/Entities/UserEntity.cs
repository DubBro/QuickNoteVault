﻿namespace QuickNoteVault.DAL.Entities;

public class UserEntity
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public ICollection<NoteEntity> Notes { get; set; } = new List<NoteEntity>();
}
