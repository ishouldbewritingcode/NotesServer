using GraphQL.Types;
using NotesServer.Models;

namespace NotesServer.GraphQL.Types;

public class UserWithNotesType : ObjectGraphType
{
    public UserWithNotesType()
    {
        Field<UserType>("user").Description("User information");
        Field<ListGraphType<NoteType>>("notes").Description("User's notes");
    }
}