var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.AspireTestApp_ApiService>("apiservice");

builder.AddProject<Projects.AspireTestApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(redis);

builder.Build().Run();
