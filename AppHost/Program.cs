using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<ApiService>("api-service");

builder.Build().Run();
