using AutoMapper;
using Base.Utils;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShipperService.Application.EventPublishers;
using ShipperService.Domain.Entities;
using ShipperService.Infrastructure.Kafka.ConfigModels;
using ShipperService.Infrastructure.Models.ShippingOrders;
using System;
using System.Threading.Tasks;

namespace ShipperService.Infrastructure.EventPublishers
{
    public class ShippingOrderEventPublisher : IShippingOrderEventPublisher
    {
        private readonly IProducer<string, string> _producer;
        private readonly IMapper _mapper;
        private readonly ILogger<ShippingOrderEventPublisher> _logger;

        private readonly string _topic;

        public ShippingOrderEventPublisher(
            KafkaEventPublisherConfigModel configModel,
            IMapper mapper,
            ILogger<ShippingOrderEventPublisher> logger)
        {
            _producer = new ProducerBuilder<string, string>(configModel.Config).Build();
            _topic = configModel.TopicMapping["ShippingOrder"];
            _mapper = mapper;
            _logger = logger;
        }

        public async Task PublishAsync(ShippingOrder order)
        {
            try
            {
                var orderModel = _mapper.Map<ShippingOrderWithExtraInfo>(order);
                await _producer.ProduceAsync(_topic, new Message<string, string>
                {
                    Key = orderModel.Id.ToString(),
                    Value = JsonConvert.SerializeObject(orderModel, NewtonJsonSerializerSettings.CAMEL),
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
