using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Messaging

var messaging = builder.AddRabbitMQ("messaging");

// Databases

var postgres = builder.AddPostgres("postgres").WithPgAdmin();
var catalogDb = postgres.AddDatabase("CatalogDB");


// Cache
var redis = builder.AddRedis("cache");

// DB Manager Apps

builder.AddProject<Catalog_Data_Manager>("catalog-db-mgr")
    .WithReference(catalogDb);


// API Apps

var catalogApi = builder.AddProject<Catalog_API>("catalog-api")
    .WithReference(catalogDb)
    .WithReference(messaging)
    .WithReference(redis);

// Apps

builder.AddProject<WebApp>("webapp")
    .WithReference(catalogApi)
    .WithReference(redis);

// App RabbitConsumer

builder.AddProject<RabbitConsumer>("consumers")
    .WithReference(messaging);

// Inject assigned URLs for Catalog API
catalogApi.WithEnvironment("CatalogOptions__PicBaseAddress", catalogApi.GetEndpoint("http"));

builder.Build().Run();
