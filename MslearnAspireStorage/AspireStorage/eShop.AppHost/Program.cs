using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator();

var queues = storage.AddQueues("AzureWebJobsStorage");

// Databases

var postgres = builder.AddPostgres("postgres").WithPgAdmin();
var catalogDb = postgres.AddDatabase("CatalogDB");

// DB Manager Apps

builder.AddProject<Catalog_Data_Manager>("catalog-db-mgr")
    .WithReference(catalogDb);

// API Apps

var catalogApi = builder.AddProject<Catalog_API>("catalog-api")
    .WithReference(queues)
    .WithReference(catalogDb);

// Apps

builder.AddProject<WebApp>("webapp")
    .WithReference(catalogApi);

// WorkService

var eshopMessage = builder.AddProject<eShop_MessageProcessor>("eshop-messageprocessor")
                          .WithReference(queues);

// Inject assigned URLs for Catalog API
catalogApi.WithEnvironment("CatalogOptions__PicBaseAddress", () => catalogApi.GetEndpoint("http").Url);


// When extrange problem the connection string was renamed and replace the endpoint and deff of protocol
// for that reason I set the conn string manualy in this seccion.
catalogApi.WithEnvironment("ConnectionStrings__AzureWebJobsStorage", "DefaultEndpointsProtocol=https;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;QueueEndpoint=https://127.0.0.1:10011/devstoreaccount1;");
eshopMessage.WithEnvironment("ConnectionStrings__AzureWebJobsStorage", "DefaultEndpointsProtocol=https;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;QueueEndpoint=https://127.0.0.1:10011/devstoreaccount1;");


builder.Build().Run();
