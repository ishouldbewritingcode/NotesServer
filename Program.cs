using GraphQL;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using NotesServer.GraphQL;
using NotesServer.GraphQL.Types;
using NotesServer.Services;

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
        Variables = request.Variables ?? new Inputs(new Dictionary<string, object?>())
    });

    await context.Response.WriteAsJsonAsync(result);
}).WithName("GraphQL");

app.Run();

// Request model for GraphQL queries
public class GraphQLRequest
{
    public string? Query { get; set; }
    public string? OperationName { get; set; }
    public Inputs? Variables { get; set; }
}

