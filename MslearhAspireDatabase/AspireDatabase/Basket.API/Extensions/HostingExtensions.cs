using eShop.Basket.API.Storage;

namespace Microsoft.Extensions.Hosting;

public static class HostingExtensions
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddDefaultAuthentication();

        // Replace the old redis to mongoDB
        //builder.AddRedisClient("BasketStore");

        builder.AddMongoDBClient("BasketDB");

        // Replace the old RedisBasketStore to MongoBasketStore
        //builder.Services.AddSingleton<RedisBasketStore>();

        builder.Services.AddSingleton<MongoBasketStore>();

        return builder;
    }
}
