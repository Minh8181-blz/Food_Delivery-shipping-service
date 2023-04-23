using Confluent.Kafka;
using System.Collections.Generic;

namespace ShipperService.Infrastructure.Kafka.ConfigModels
{
    public class KafkaConsumerConfigModel
    {
        public Dictionary<string, KafkaSingleConsumerConfig> ConfigMapping { get; set; }
    }
}
