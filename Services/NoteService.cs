using Microsoft.Data.Sqlite;
using NotesServer.Models;

namespace NotesServer.Services;

public class NoteService : INoteService
{
    private readonly string _connectionString;

    public NoteService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=ReactTanstackNotes.db";
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, Email FROM User WHERE Email = @email";
            command.Parameters.AddWithValue("@email", email);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = Guid.Parse(reader.GetString(0)),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2)
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user by email: {ex.Message}");
        }

        return null;
    }

    public async Task<IEnumerable<Note>> GetAllNotes()
    {
        var notes = new List<Note>();

        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Note, UserId FROM Note";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var note = new Note
                {
                    Id = Guid.Parse(reader.GetString(0)),
                    Text = reader.GetString(1),
                    UserId = Guid.Parse(reader.GetString(2))
                };
                notes.Add(note);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching notes: {ex.Message}");
        }

        return notes;
    }

    public async Task<User?> GetUserById(Guid id)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, Email FROM User WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id.ToString());

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = Guid.Parse(reader.GetString(0)),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2)
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user: {ex.Message}");
        }

        return null;
    }

    public async Task<IEnumerable<Note>> GetNotesByUserId(Guid userId)
    {
        var notes = new List<Note>();

        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Note, UserId FROM Note WHERE UserId = @userId";
            command.Parameters.AddWithValue("@userId", userId.ToString());

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var note = new Note
                {
                    Id = Guid.Parse(reader.GetString(0)),
                    Text = reader.GetString(1),
                    UserId = Guid.Parse(reader.GetString(2))
                };
                notes.Add(note);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching notes for user: {ex.Message}");
        }

        return notes;
    }

    public async Task<(User?, IEnumerable<Note>)> GetUserWithNotesByEmail(string email)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            // Get user
            using var userCommand = connection.CreateCommand();
            userCommand.CommandText = "SELECT Id, Name, Email FROM User WHERE Email = @email";
            userCommand.Parameters.AddWithValue("@email", email);

            User? user = null;
            using var userReader = await userCommand.ExecuteReaderAsync();
            if (await userReader.ReadAsync())
            {
                user = new User
                {
                    Id = Guid.Parse(userReader.GetString(0)),
                    Name = userReader.GetString(1),
                    Email = userReader.GetString(2)
                };
            }

            if (user == null)
                return (null, Enumerable.Empty<Note>());

            // Get notes for user
            using var notesCommand = connection.CreateCommand();
            notesCommand.CommandText = "SELECT Id, Note, UserId FROM Note WHERE UserId = @userId";
            notesCommand.Parameters.AddWithValue("@userId", user.Id.ToString());

            var notes = new List<Note>();
            using var notesReader = await notesCommand.ExecuteReaderAsync();
            while (await notesReader.ReadAsync())
            {
                notes.Add(new Note
                {
                    Id = Guid.Parse(notesReader.GetString(0)),
                    Text = notesReader.GetString(1),
                    UserId = Guid.Parse(notesReader.GetString(2))
                });
            }

            return (user, notes);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user with notes: {ex.Message}");
            return (null, Enumerable.Empty<Note>());
        }
    }
}
