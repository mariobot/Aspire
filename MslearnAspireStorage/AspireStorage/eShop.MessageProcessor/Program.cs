using eShop.MessageProcessor;

var builder = Host.CreateApplicationBuilder(args);

builder.AddAzureQueueClient("AzureWebJobsStorage");

builder.Services.AddHostedService<WorkerService>();

var host = builder.Build();
host.Run();
