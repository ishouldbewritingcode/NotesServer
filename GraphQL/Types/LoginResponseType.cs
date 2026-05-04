using GraphQL.Types;
using NotesServer.Models;

namespace NotesServer.GraphQL.Types;

public class UserInfoType : ObjectGraphType<UserInfo>
{
    public UserInfoType()
    {
        Field(x => x.Id).Description("User ID");
        Field(x => x.Email).Description("User email");
    }
}

public class LoginResponseType : ObjectGraphType<LoginResponse>
{
    public LoginResponseType()
    {
        Field(x => x.Token).Description("JWT authentication token");
        Field<UserInfoType>("user", "User information");
    }
}
