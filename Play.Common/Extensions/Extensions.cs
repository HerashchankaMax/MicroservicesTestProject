using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Common.Interfaces;
using Play.Common.MongoDB;
using Play.Common.Settings;

namespace Play.Common.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeSerializer(BsonType.String));
            return services.AddSingleton(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var mongoSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                var client = new MongoClient(mongoSettings.ConnectionString);
                return client.GetDatabase(serviceSettings.ServiceName);
            });
        }

        public static IServiceCollection RegisterDatabase<T>(this IServiceCollection services, string databaseName) where T : IEntity
        {
            return services.AddSingleton<IRepository<T>>(provider =>
            {
                var database = provider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database, databaseName);
            });

        }

        public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(Assembly.GetEntryAssembly());
                x.UsingRabbitMq((context, configurator) =>
                {
                    var conf = context.GetService<IConfiguration>();
                    var serviceSettings = conf.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    var rabbitMqSettings = conf.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();

                    configurator.Host(rabbitMqSettings.Host);
                    configurator.ConfigureEndpoints(context,
                        new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                });
            });
            services.AddMassTransitHostedService();
            return services;
        }
    }
}
