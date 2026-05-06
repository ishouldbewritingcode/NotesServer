using GraphQL;
using GraphQL.Types;
using NotesServer.GraphQL.Types;
using NotesServer.Services;

namespace NotesServer.GraphQL;

public class AppMutation : ObjectGraphType
{
    public AppMutation(IAuthenticationService authService, INoteService noteService)
    {
        Field<LoginResponseType>("login")
            .Argument<NonNullGraphType<StringGraphType>>("email", "User email")
            .Argument<StringGraphType>("name", "Display name, required when creating a new account")
            .Description("Login with email. Returns requiresName=true for new users until a name is provided.")
            .ResolveAsync(async context =>
            {
                var email = context.GetArgument<string>("email");
                var name = context.GetArgument<string?>("name");

                return await authService.LoginAsync(email, name);
            });

        Field<NoteType>("createNote")
            .Argument<NonNullGraphType<IdGraphType>>("userId", "ID of the owning user")
            .Argument<NonNullGraphType<StringGraphType>>("title", "Note title")
            .Argument<NonNullGraphType<StringGraphType>>("text", "Note text")
            .Description("Create a new note")
            .ResolveAsync(async context =>
            {
                var userId = context.GetArgument<Guid>("userId");
                var title = context.GetArgument<string>("title");
                var text = context.GetArgument<string>("text");
                return await noteService.CreateNote(userId, title, text);
            });

        Field<NoteType>("updateNote")
            .Argument<NonNullGraphType<IdGraphType>>("id", "Note ID")
            .Argument<NonNullGraphType<StringGraphType>>("title", "Note title")
            .Argument<NonNullGraphType<StringGraphType>>("text", "Note text")
            .Description("Update an existing note")
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<Guid>("id");
                var title = context.GetArgument<string>("title");
                var text = context.GetArgument<string>("text");
                var result = await noteService.UpdateNote(id, title, text);
                if (result == null)
                    throw new ExecutionError("Note not found");
                return result;
            });

        Field<BooleanGraphType>("deleteNote")
            .Argument<NonNullGraphType<IdGraphType>>("id", "Note ID")
            .Description("Delete a note")
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<Guid>("id");
                return (object?)await noteService.DeleteNote(id);
            });
    }
}
