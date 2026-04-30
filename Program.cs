using NotesServer.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQLServices();

var app = builder.Build();          

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.MapGraphQLEndpoint();

app.Run();

