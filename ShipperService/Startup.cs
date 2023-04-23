using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShipperService.Infrastructure.DependencyRegistrations;
using ShipperService.Infrastructure.Kafka.ConfigModels;
using ShipperService.Infrastructure.Redis;
using ShipperService.Presentation.AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace ShipperService
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSqlServerDbContext(Configuration.GetConnectionString("SqlServerDb"));
            services.AddIdentityServer(Environment, "Bearer", Configuration.GetValue<string>("AuthIdentityOrigin"));
            services.AddRedisConnection(new RedisConnectionConfigModel
            {
                EndPoint = Configuration.GetValue<string>("Redis:EndPoint"),
                User = Configuration.GetValue<string>("Redis:User"),
                Password = Configuration.GetValue<string>("Redis:Password"),
            });

            string kafkaBootStrapServers = Configuration.GetValue<string>("Kafka:BootstrapServers");
            string kafkaUsername = Configuration.GetValue<string>("Kafka:SaslUsername");
            string kafkaSalsPassword = Configuration.GetValue<string>("Kafka:SaslPassword");
            string consumerGroup = Configuration.GetValue<string>("Kafka:ConsumerGroup");

            services.AddKafkaEventPublisherConfig(new KafkaEventPublisherConfigModel
            {
                Config = new ProducerConfig
                {
                    BootstrapServers = kafkaBootStrapServers,
                    SaslUsername = kafkaUsername,
                    SaslPassword = kafkaSalsPassword,
                    SecurityProtocol = SecurityProtocol.SaslSsl,
                    SaslMechanism = SaslMechanism.ScramSha512,
                },
                TopicMapping = Configuration.GetSection("Kafka:PubTopicMapping")
                    .GetChildren()
                    .ToDictionary(x => x.Key, x => x.Value),
            });
            services.AddKafkaConsumers(new KafkaConsumerConfigModel
            {
                ConfigMapping = Configuration
                    .GetSection("Kafka:SubTopicMapping")
                    .GetChildren()
                    .ToDictionary(
                        x => x.Key,
                        x => new KafkaSingleConsumerConfig
                        {
                            Topic = x.Value,
                            Config = new ConsumerConfig
                            {
                                BootstrapServers = kafkaBootStrapServers,
                                SaslUsername = kafkaUsername,
                                SaslPassword = kafkaSalsPassword,
                                SecurityProtocol = SecurityProtocol.SaslSsl,
                                SaslMechanism = SaslMechanism.ScramSha512,
                                GroupId = consumerGroup,
                                EnableAutoCommit = false,
                                AutoOffsetReset = AutoOffsetReset.Earliest,
                            }
                        })
            });
            services.AddApplicationLayerDelegateServices();
            services.AddDatabaseAccessServices();
            services.AddInfrastructureLayerServices();
            services.AddAutoMapperService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
