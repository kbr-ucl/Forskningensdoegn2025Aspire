var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/hello", () =>
{
    app.Logger.LogInformation("ServiceB got Hello request");
    var greeting = new HelloResponse("Hello from ServiceB");
    return greeting;
});

app.Run();

internal record HelloResponse(string Greeting)
{
}
