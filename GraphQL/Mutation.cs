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
            .Argument<NonNullGraphType<StringGraphType>>("password", "User password")
            .Description("Login with email and password")
            .ResolveAsync(async context =>
            {
                var email = context.GetArgument<string>("email");
                var password = context.GetArgument<string>("password");

                var result = await authService.LoginAsync(email, password);
                if (result == null)
                {
                    throw new ExecutionError("Invalid email or password");
                }

                return result;
            });
    }
}
