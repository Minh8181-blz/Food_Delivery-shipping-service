using Confluent.Kafka;

namespace ShipperService.Infrastructure.Kafka.ConfigModels
{
    public class KafkaSingleConsumerConfig
    {
        public string Topic { get; set; }
        public ConsumerConfig Config { get; set; }
    }
}
