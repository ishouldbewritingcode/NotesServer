using GraphQL;
using GraphQL.Execution;
using GraphQL.Types;
using NotesServer.GraphQL;
using NotesServer.GraphQL.Types;
using NotesServer.Services;

namespace NotesServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGraphQLServices(this IServiceCollection services)
    {
        services.AddScoped<AppQuery>();
        services.AddScoped<AppMutation>();
        services.AddScoped<AppSchema>();
        services.AddScoped<UserType>();
        services.AddScoped<NoteType>();
        services.AddScoped<UserWithNotesType>();
        services.AddScoped<LoginResponseType>();
        services.AddScoped<UserInfoType>();
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IDocumentExecuter, DocumentExecuter>();

        return services;
    }
}