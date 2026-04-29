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
}
