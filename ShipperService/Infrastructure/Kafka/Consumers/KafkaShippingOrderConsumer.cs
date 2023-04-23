using Base.Utils;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShipperService.Application.Commands.FindShipper;
using ShipperService.Infrastructure.Kafka.ConfigModels;
using ShipperService.Infrastructure.Models.ShippingOrders;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShipperService.Infrastructure.Kafka.Consumers
{
    public class KafkaShipperMatchCommandConsumer : BackgroundService
    {
        private readonly KafkaSingleConsumerConfig _consumerConfig;
        private readonly IServiceProvider _services;
        private readonly ILogger<KafkaShipperMatchCommandConsumer> _logger;

        private IMediator _mediator;

        public KafkaShipperMatchCommandConsumer(
            KafkaConsumerConfigModel configModel,
            IServiceProvider services,
            ILogger<KafkaShipperMatchCommandConsumer> logger)
        {
            _consumerConfig = configModel.ConfigMapping["ShippingOrder"];
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () => {
                using var consumer = new ConsumerBuilder<string, string>(_consumerConfig.Config).Build();
                consumer.Subscribe(_consumerConfig.Topic);
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume();
                    Console.WriteLine(consumeResult.Message.Value +
                            " P:" + consumeResult.Partition.Value +
                            " Offset:" + consumeResult.Offset.Value);
                    try
                    {
                        using var serviceScope = _services.GetRequiredService<IServiceScopeFactory>().CreateScope();
                        _mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
                        var orderDto = JsonConvert.DeserializeObject<ShippingOrderWithExtraInfo>(
                            consumeResult.Message.Value,
                            NewtonJsonSerializerSettings.INCLUDE_INHERITED_PROPS);
                        var result = await HandleShippingOrderAsync(orderDto);
                        consumer.Commit(consumeResult);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        consumer.Seek(new TopicPartitionOffset(_consumerConfig.Topic, consumeResult.Partition, consumeResult.Offset));
                    }
                }

                consumer.Close();
            });
        }

        private async Task<bool> HandleShippingOrderAsync(ShippingOrderWithExtraInfo order)
        {
            if (order != null && order.DomainEvents != null)
            {
                if (order.DomainEvents.Any(de => de.Name == "shipping_order_placed"))
                {
                    return await CreateShipperMatchCommandAsync(order);
                }
            }

            return false;
        }

        private async Task<bool> CreateShipperMatchCommandAsync(ShippingOrderWithExtraInfo order)
        {
            await _mediator.Send(new FindShipperCommand
            {
                ShippingOrder = order,
            });
            return true;
        }
    }
}
