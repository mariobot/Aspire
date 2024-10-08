var builder = DistributedApplication.CreateBuilder(args);

// Databases

// Not using redis
//var basketStore = builder.AddRedis("BasketStore").WithRedisCommander();

var postgres = builder.AddPostgres("postgres")
    .WithEnvironment("POSTGRES_DB", "CatalogDB")
    .WithBindMount("../Catalog.API/Seed", "/docker-entrypoint-initdb.d")
    .WithPgAdmin();
var catalogDB = postgres.AddDatabase("CatalogDB");

var mongo = builder.AddMongoDB("mongodb")
    .WithMongoExpress()
    .AddDatabase("BasketDB");

// Identity Providers

//var idp = builder.AddKeycloakContainer("idp", tag: "23.0")
//   .ImportRealms("../Keycloak/data/import");

// DB Manager Apps is not using
//builder.AddProject<Projects.Catalog_Data_Manager>("catalog-db-mgr");

// API Apps

var catalogApi = builder.AddProject<Projects.Catalog_API>("catalog-api")
    .WithReference(catalogDB);

var basketApi = builder.AddProject<Projects.Basket_API>("basket-api")
        .WithReference(mongo);
//.WithReference(idp);

// Apps

// Force HTTPS profile for web app (required for OIDC operations)
var webApp = builder.AddProject<Projects.WebApp>("webapp")
    .WithReference(basketApi)
    .WithReference(catalogApi);
    //.WithReference(idp, env: "Identity__ClientSecret");

// Inject the project URLs for Keycloak realm configuration
var webAppHttp = webApp.GetEndpoint("http");
var webAppHttps = webApp.GetEndpoint("https");
/*idp.WithEnvironment("WEBAPP_HTTP_CONTAINERHOST", webAppHttp);
idp.WithEnvironment("WEBAPP_HTTP", () => $"{webAppHttp.Scheme}://{webAppHttp.Host}:{webAppHttp.Port}");
if (webAppHttps.Exists)
{
  idp.WithEnvironment("WEBAPP_HTTPS_CONTAINERHOST", webAppHttps);
  idp.WithEnvironment("WEBAPP_HTTPS", () => $"{webAppHttps.Scheme}://{webAppHttps.Host}:{webAppHttps.Port}");
}
else
{
  // Still need to set these environment variables so the KeyCloak realm import doesn't fail
  idp.WithEnvironment("WEBAPP_HTTPS_CONTAINERHOST", webAppHttp);
  idp.WithEnvironment("WEBAPP_HTTPS", () => $"{webAppHttp.Scheme}://{webAppHttp.Host}:{webAppHttp.Port}");
}*/

// Inject assigned URLs for Catalog API
catalogApi.WithEnvironment("CatalogOptions__PicBaseAddress", catalogApi.GetEndpoint("http"));

builder.Build().Run();
