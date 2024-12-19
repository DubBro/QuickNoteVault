﻿namespace QuickNoteVault.DAL.Entities;

public class NoteEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public UserEntity User { get; set; } = new UserEntity();
}