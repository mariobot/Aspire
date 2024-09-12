using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitConsumer;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddRabbitMQClient("messaging");

builder.Services.AddHostedService<CatalogProcessingJob>();

builder.Services.AddHostedService<CatalogItemProcessingJob>();

var host = builder.Build();

host.Run();