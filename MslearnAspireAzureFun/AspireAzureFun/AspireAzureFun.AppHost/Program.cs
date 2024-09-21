using Aspire.Hosting.Azure;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage").RunAsEmulator();
var queue = storage.AddQueues("queue");
var blob = storage.AddBlobs("blob");

builder.AddAzureFunctionsProject<Projects.AspireAzureFun_Function>("azurefunction")
    .WithReference(queue)
    .WithReference(blob);


//var azurefun = builder.AddProject<Projects.>("azurefun");

/*builder.AddProject<Projects.AspireAzureFun_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);*/

builder.Build().Run();
