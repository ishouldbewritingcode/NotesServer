using GraphQL.Types;
using NotesServer.Models;

namespace NotesServer.GraphQL.Types;

public class UserType : ObjectGraphType<User>
{
    public UserType()
    {
        Field(x => x.Id).Description("User ID");
        Field(x => x.Name).Description("User's name");
        Field(x => x.Email).Description("User's email address");
    }
}
