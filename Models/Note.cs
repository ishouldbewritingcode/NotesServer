namespace NotesServer.Models;

public class Note
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}
