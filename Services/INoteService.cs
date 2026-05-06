using NotesServer.Models;

namespace NotesServer.Services;

public interface INoteService
{
    Task<User?> GetUserByEmail(string email);
    Task<User> CreateUser(string email, string name);
    Task<IEnumerable<Note>> GetAllNotes();
    Task<User?> GetUserById(Guid id);
    Task<IEnumerable<Note>> GetNotesByUserId(Guid userId);
    Task<(User?, IEnumerable<Note>)> GetUserWithNotesByEmail(string email);
    Task<Note> CreateNote(Guid userId, string title, string text);
    Task<Note?> UpdateNote(Guid id, string title, string text);
    Task<bool> DeleteNote(Guid id);
}
