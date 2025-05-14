var builder = DistributedApplication.CreateBuilder(args);

var cs = builder.AddConnectionString("Ordering");
builder
    .AddProject<Projects.Company_Ordering_API>("company-ordering-api")
    .WithExternalHttpEndpoints()
    .WithReference(cs);


builder.Build().Run();