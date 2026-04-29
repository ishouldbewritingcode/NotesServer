using GraphQL;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using NotesServer.GraphQL;
using NotesServer.GraphQL.Types;
using NotesServer.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<AppQuery>();
builder.Services.AddScoped<AppSchema>();
builder.Services.AddScoped<UserType>();
builder.Services.AddScoped<NoteType>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IDocumentExecuter, DocumentExecuter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Map GraphQL endpoint
app.MapPost("/graphql", async (HttpContext context, AppSchema schema, IDocumentExecuter executer) =>
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
        Variables = request.Variables != null ? new Inputs(ConvertVariables(request.Variables)) : new Inputs(new Dictionary<string, object>())
    });

    var serializer = new GraphQL.SystemTextJson.GraphQLSerializer();
    var json = serializer.Serialize(result);
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(json);
}).WithName("GraphQL");

app.Run();

Dictionary<string, object> ConvertVariables(Dictionary<string, object> variables)
{
    var result = new Dictionary<string, object>();
    foreach (var kvp in variables)
    {
        if (kvp.Value is JsonElement je)
        {
            result[kvp.Key] = je.ValueKind switch
            {
                JsonValueKind.String => je.GetString(),
                JsonValueKind.Number => je.TryGetInt32(out int i) ? i : je.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => je.ToString() // for arrays/objects, but for now
            };
        }
        else
        {
            result[kvp.Key] = kvp.Value;
        }
    }
    return result;
}

// Request model for GraphQL queries
public class GraphQLRequest
{
    public string? Query { get; set; }
    public string? OperationName { get; set; }
    public Dictionary<string, object>? Variables { get; set; }
}

