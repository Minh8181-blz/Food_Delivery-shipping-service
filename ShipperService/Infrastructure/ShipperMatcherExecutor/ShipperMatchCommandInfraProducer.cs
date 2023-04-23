using Base.Utils;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShipperService.Infrastructure.Kafka.ConfigModels;
using ShipperService.Infrastructure.Models.ShipperAssignments;
using System;
using System.Threading.Tasks;

namespace ShipperService.Infrastructure.ShipperMatcherExecutor
{
    public class ShipperMatchCommandInfraProducer
    {
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<ShipperMatchCommandInfraProducer> _logger;

        private readonly string _topic;

        public ShipperMatchCommandInfraProducer(
            KafkaEventPublisherConfigModel configModel,
            ILogger<ShipperMatchCommandInfraProducer> logger)
        {
            _producer = new ProducerBuilder<string, string>(configModel.Config).Build();
            _topic = configModel.TopicMapping["ShipperMatchCommand"];
            _logger = logger;
        }

        public async Task PublishAsync(GeoHashShipperMatchCommand command)
        {
            try
            {   
                await _producer.ProduceAsync(_topic, new Message<string, string>
                {
                    Key = command.ShippingOrderId.ToString(),
                    Value = JsonConvert.SerializeObject(command, NewtonJsonSerializerSettings.CAMEL),
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
