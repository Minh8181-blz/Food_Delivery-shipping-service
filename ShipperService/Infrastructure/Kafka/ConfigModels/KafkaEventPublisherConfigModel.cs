using Confluent.Kafka;
using System.Collections.Generic;

namespace ShipperService.Infrastructure.Kafka.ConfigModels
{
    public class KafkaEventPublisherConfigModel
    {
        public ProducerConfig Config { get; set; }
        public Dictionary<string, string> TopicMapping { get; set; }
    }
}
