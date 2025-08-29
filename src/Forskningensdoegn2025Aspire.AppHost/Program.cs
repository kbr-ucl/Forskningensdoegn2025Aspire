var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume(name: "SqlData");


var serviceASqlDb = sql.AddDatabase("serviceADb");
var serviceBSqlDb = sql.AddDatabase("serviceBDb");

var serviceA = builder.AddProject<Projects.ServiceA>("servicea")
    .WithReference(serviceASqlDb)
    .WaitFor(serviceASqlDb);

var serviceB = builder.AddProject<Projects.ServiceB>("serviceb")
    .WithReference(serviceBSqlDb)
    .WaitFor(serviceBSqlDb);

var gateway = builder.AddProject<Projects.Gateway>("gateway")
    .WithExternalHttpEndpoints()
    .WithReference(serviceA)
    .WithReference(serviceB)
    .WaitFor(serviceA)
    .WaitFor(serviceB);

// builder.AddDockerComposePublisher();
// builder.AddDockerComposeEnvironment("docker-compose");

builder.AddProject<Projects.MvcFrontend>("mvcfrontend")
    .WithReference(gateway)
    .WaitFor(gateway);


builder.Build().Run();
