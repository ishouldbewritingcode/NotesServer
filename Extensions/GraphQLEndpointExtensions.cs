using GraphQL;
using GraphQL.Execution;
using GraphQL.SystemTextJson;
using NotesServer.GraphQL;
using NotesServer.Models;
using NotesServer.Services.GraphQL;

namespace NotesServer.Extensions;

public static class GraphQLEndpointExtensions
{
    public static void MapGraphQLEndpoint(this WebApplication app)
    {
        app.MapPost("/graphql", HandleGraphQLRequest)
            .WithName("GraphQL");
    }

    private static async Task HandleGraphQLRequest(
        HttpContext context, 
        AppSchema schema, 
        IDocumentExecuter executer)
    {
        var request = await context.Request.ReadFromJsonAsync<GraphQLRequest>();
        if (request == null)
        {
            context.Response.StatusCode = 400;
            return;
        }

        var result = await executer.ExecuteAsync(new ExecutionOptions
        {
            Schema = schema,
            Query = request.Query,
            OperationName = request.OperationName,
            Variables = request.Variables != null 
                ? new Inputs(GraphQLVariableConverter.ConvertVariables(request.Variables)) 
                : new Inputs(new Dictionary<string, object>())
        });

        var serializer = new GraphQLSerializer();
        var json = serializer.Serialize(result);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(json);
    }
}