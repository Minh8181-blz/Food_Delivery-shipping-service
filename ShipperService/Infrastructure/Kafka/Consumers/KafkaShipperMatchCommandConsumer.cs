using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShipperService.Infrastructure.Database;
using ShipperService.Infrastructure.Kafka.ConfigModels;
using ShipperService.Infrastructure.Models.ShipperAssignments;
using ShipperService.Infrastructure.ShipperMatcherExecutor;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShipperService.Infrastructure.Kafka.Consumers
{
    public class KafkaShippingOrderConsumer : BackgroundService
    {
        private readonly KafkaSingleConsumerConfig _consumerConfig;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<KafkaOrderConsumer> _logger;
        private readonly ShipperMatcher _shipperMatcher;

        private IMediator _mediator;

        public KafkaShippingOrderConsumer(
            KafkaConsumerConfigModel configModel,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<KafkaOrderConsumer> logger,
            ShipperMatcher shipperMatcher)
        {
            _consumerConfig = configModel.ConfigMapping["ShipperMatchCommand"];
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _shipperMatcher = shipperMatcher;
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
                        using var serviceScope = _serviceScopeFactory.CreateScope();
                        var matchCommand = JsonConvert.DeserializeObject<GeoHashShipperMatchCommand>(consumeResult.Message.Value);
                        var result = await HandleCommandAsync(matchCommand, serviceScope);
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

        private Task<bool> HandleCommandAsync(GeoHashShipperMatchCommand matchCommand, IServiceScope serviceScope)
        {
            return SendMatchCommandAsync(matchCommand, serviceScope);
        }

        private async Task<bool> SendMatchCommandAsync(GeoHashShipperMatchCommand matchCommand, IServiceScope serviceScope)
        {
            _mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
            await _shipperMatcher.BindCustomerMediatorService(_mediator).FindShipperForShippingOrder(matchCommand);
            return true;
        }
    }
}
