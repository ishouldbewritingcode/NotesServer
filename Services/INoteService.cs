using NotesServer.Models;

namespace NotesServer.Services;

public interface INoteService
{
    Task<User?> GetUserByEmail(string email);
    Task<IEnumerable<Note>> GetAllNotes();
    Task<User?> GetUserById(Guid id);
    Task<IEnumerable<Note>> GetNotesByUserId(Guid userId);
    Task<(User?, IEnumerable<Note>)> GetUserWithNotesByEmail(string email);
}
