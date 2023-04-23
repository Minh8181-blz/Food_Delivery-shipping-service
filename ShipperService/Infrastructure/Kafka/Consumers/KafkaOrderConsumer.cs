using Confluent.Kafka;
using FoodEstablishment.Infrastructure.Models.Orders;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShipperService.Application.Commands.CreateAcceptedShippingOrder;
using ShipperService.Application.Dtos.ShippingOrders;
using ShipperService.Infrastructure.Kafka.ConfigModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShipperService.Infrastructure.Kafka.Consumers
{
    public class KafkaOrderConsumer : BackgroundService
    {
        private readonly KafkaSingleConsumerConfig _consumerConfig;
        private readonly IServiceProvider _services;
        private readonly ILogger<KafkaOrderConsumer> _logger;

        private IMediator _mediator;

        public KafkaOrderConsumer(
            KafkaConsumerConfigModel configModel,
            IServiceProvider services,
            ILogger<KafkaOrderConsumer> logger)
        {
            _consumerConfig = configModel.ConfigMapping["Order"];
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
                        var orderDto = JsonConvert.DeserializeObject<OrderWithExtraInfo>(consumeResult.Message.Value);
                        var result = await HandleOrderAsync(orderDto);
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

        private async Task<bool> HandleOrderAsync(OrderWithExtraInfo order)
        {
            if (order != null && order.DomainEvents != null)
            {
                if (order.DomainEvents.Any(de => de.Name == "order_placed"))
                {
                    return await CreateShippingOrderAsync(order);
                }
            }

            return false;
        }

        private async Task<bool> CreateShippingOrderAsync(OrderWithExtraInfo order)
        {
            var commandDto = new CreateShippingOrderCommandDto
            {
                OrderId = order.Id,
                FromLatitude = order.FromLatitude,
                FromLongitude = order.FromLongitude,
                ToLatitude = order.ShippingLatitude,
                ToLongitude = order.ShippingLongitude,
            };
            var command = new CreateAcceptedShippingOrderCommand(commandDto);
            var result = await _mediator.Send(command);
            return result.Succeeded;
        }
    }
}
