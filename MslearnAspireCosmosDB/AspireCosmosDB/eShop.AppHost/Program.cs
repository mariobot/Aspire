using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var connectionstring = builder.AddConnectionString("localcosmosdb");

// Databases

var cosmos = builder.AddAzureCosmosDB("cosmos").RunAsEmulator();
var cosmosdb = cosmos.AddDatabase("catalogcosmosdb");

var postgres = builder.AddPostgres("postgres").WithPgAdmin();
var catalogDb = postgres.AddDatabase("CatalogDB");

// DB Manager Apps

builder.AddProject<Catalog_Data_Manager>("catalog-db-mgr")
    .WithReference(catalogDb);

// API Apps

var catalogApi = builder.AddProject<Catalog_API>("catalog-api")
    .WithReference(catalogDb)
    .WithReference(connectionstring)
    .WithReference(cosmosdb);

// Apps

builder.AddProject<WebApp>("webapp")
    .WithReference(catalogApi);


// Inject assigned URLs for Catalog API
catalogApi.WithEnvironment("CatalogOptions__PicBaseAddress", catalogApi.GetEndpoint("http"));

builder.Build().Run();
