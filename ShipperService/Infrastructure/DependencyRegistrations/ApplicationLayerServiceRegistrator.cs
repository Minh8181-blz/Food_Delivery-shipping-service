using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ShipperService.Application.EventPublishers;
using ShipperService.Application.Services;
using ShipperService.Infrastructure.AppServices;
using ShipperService.Infrastructure.EventPublishers;
using ShipperService.Infrastructure.Kafka.ConfigModels;
using ShipperService.Infrastructure.Kafka.Consumers;
using ShipperService.Infrastructure.Redis;
using StackExchange.Redis;
using System.Reflection;

namespace ShipperService.Infrastructure.DependencyRegistrations
{
    public static class ApplicationLayerServiceRegistrator
    {
        public static void AddApplicationLayerDelegateServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSingleton<IShippingOrderEventPublisher, ShippingOrderEventPublisher>();
            services.AddSingleton<IShipperSessionPublisher, ShipperSessionPublisher>();
            services.AddSingleton<IShipperLocationAppService, ShipperLocationAppService>();
            services.AddSingleton<IShipperMatchCommandPublisher, ShipperMatchCommandPublisher>();
        }

        public static void AddKafkaEventPublisherConfig
            (this IServiceCollection services,
            KafkaEventPublisherConfigModel configModel)
        {
            services.AddSingleton(configModel);
        }

        public static void AddRedisConnection(this IServiceCollection services, RedisConnectionConfigModel configModel)
        {
            var multiplexer = ConnectionMultiplexer.Connect(
                new ConfigurationOptions
                {
                    EndPoints = { configModel.EndPoint },
                    User = configModel.User,
                    Password = configModel.Password,
                });
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        }

        public static void AddKafkaConsumers(
            this IServiceCollection services,
            KafkaConsumerConfigModel configModel)
        {
            services.AddSingleton(configModel);
            services.AddHostedService<KafkaOrderConsumer>();
            services.AddHostedService<KafkaShippingOrderConsumer>();
            services.AddHostedService<KafkaShipperMatchCommandConsumer>();
        }
    }
}
