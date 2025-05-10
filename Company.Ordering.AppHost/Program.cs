
var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Company_Ordering_API>("company-ordering-api");
//builder.Services

builder.Build().Run();
