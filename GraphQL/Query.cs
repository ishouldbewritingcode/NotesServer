using GraphQL;
using GraphQL.Execution;
using GraphQL.Types;
using NotesServer.GraphQL.Types;
using NotesServer.Services;

namespace NotesServer.GraphQL;

public class AppQuery : ObjectGraphType
{
    public AppQuery(INoteService noteService)
    {
        Field<ListGraphType<UserType>>("users")
            .Description("Get all users")
            .ResolveAsync(async _ => (object?)await noteService.GetAllUsers());

        Field<ListGraphType<NoteType>>("notes")
            .Description("Get all notes")
            .ResolveAsync(async _ => (object?)await noteService.GetAllNotes());

        Field<UserType>("user")
            .Argument<NonNullGraphType<IdGraphType>>("id", "User ID")
            .Description("Get a user by ID")
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<Guid>("id");
                return await noteService.GetUserById(id);
            });

        Field<ListGraphType<NoteType>>("notesByUser")
            .Argument<NonNullGraphType<IdGraphType>>("userId", "User ID")
            .Description("Get notes by user ID")
            .ResolveAsync(async context =>
            {
                var userId = context.GetArgument<Guid>("userId");
                return (object?)await noteService.GetNotesByUserId(userId);
            });
    }
}
