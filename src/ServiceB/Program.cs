using ServiceB.Model;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddControllers();

// Add services to the container.
builder.AddSqlServerDbContext<ServiceBDbContext>("serviceBDb");

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

app.MapControllers();

// While developing locally, you need to create a database inside the SQL Server container.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ServiceBDbContext>();
    context.Database.EnsureCreated();
    // Check if the database is empty and add a sample entity if it is.
    if (!context.ServiceBEntites.Any())
    {
        context.ServiceBEntites.Add(new ServiceBEntity { Name = "SampleB", Description = "Sample description B" });
        context.SaveChanges();
    }
}

app.Run();

internal record HelloResponse(string Greeting)
{
}
