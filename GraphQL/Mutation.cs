using GraphQL;
using GraphQL.Types;
using NotesServer.GraphQL.Types;
using NotesServer.Services;

namespace NotesServer.GraphQL;

public class AppMutation : ObjectGraphType
{
    public AppMutation(IAuthenticationService authService)
    {
        Field<LoginResponseType>("login")
            .Argument<NonNullGraphType<StringGraphType>>("email", "User email")
            .Description("Login with email")
            .ResolveAsync(async context =>
            {
                var email = context.GetArgument<string>("email");

                var result = await authService.LoginAsync(email);
                if (result == null)
                {
                    throw new ExecutionError("Invalid email");
                }

                return result;
            });
    }
}
