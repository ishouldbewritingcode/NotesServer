using GraphQL.Types;
using NotesServer.Models;

namespace NotesServer.GraphQL.Types;

public class NoteType : ObjectGraphType<Note>
{
    public NoteType()
    {
        Field(x => x.Id).Description("Note ID");
        Field(x => x.Text).Description("Note text content");
        Field(x => x.UserId).Description("User ID who owns this note");
    }
}
