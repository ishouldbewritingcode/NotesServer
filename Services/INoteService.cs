using NotesServer.Models;

namespace NotesServer.Services;

public interface INoteService
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<IEnumerable<Note>> GetAllNotes();
    Task<User?> GetUserById(Guid id);
    Task<IEnumerable<Note>> GetNotesByUserId(Guid userId);
}
