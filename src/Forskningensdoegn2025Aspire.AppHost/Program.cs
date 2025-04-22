var builder = DistributedApplication.CreateBuilder(args);

var serviceA = builder.AddProject<Projects.ServiceA>("servicea");

var serviceB = builder.AddProject<Projects.ServiceB>("serviceb");

var gateway = builder.AddProject<Projects.Gateway>("gateway")
    .WithExternalHttpEndpoints()
    .WithReference(serviceA)
    .WithReference(serviceB);


builder.Build().Run();
