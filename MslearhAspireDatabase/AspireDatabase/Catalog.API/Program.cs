using eShop.Catalog.API;
using eShop.Catalog.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultOpenApi();
builder.AddApplicationServices();

builder.Services.AddProblemDetails();

// Old deff of context using SqlLite
//builder.Services.AddDbContext<CatalogDbContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("sqlconnection") ?? throw new InvalidOperationException("Connection string 'sqlconnection' not found.")));

builder.AddNpgsqlDbContext<CatalogDbContext>("CatalogDB");

var app = builder.Build();

app.UseDefaultOpenApi();

app.MapDefaultEndpoints();

app.MapGroup(app.GetOptions<CatalogOptions>().ApiBasePath)
    .WithTags("Catalog API")
    .MapCatalogApi();

app.Run();
