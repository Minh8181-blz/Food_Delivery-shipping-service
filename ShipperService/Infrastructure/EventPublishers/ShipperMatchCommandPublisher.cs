using AutoMapper;
using Base.Utils;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShipperService.Application.Dtos.ShipperAssignment;
using ShipperService.Application.EventPublishers;
using ShipperService.Domain.Entities;
using ShipperService.Infrastructure.Helpers;
using ShipperService.Infrastructure.Kafka.ConfigModels;
using ShipperService.Infrastructure.Models.ShipperAssignments;
using ShipperService.Infrastructure.Models.ShippingOrders;
using ShipperService.Infrastructure.ShipperMatcherExecutor;
using ShipperService.Utils;
using System;
using System.Threading.Tasks;

namespace ShipperService.Infrastructure.EventPublishers
{
    public class ShipperMatchCommandPublisher : IShipperMatchCommandPublisher
    {
        private readonly ShipperMatchCommandInfraProducer _infraProducer;

        public ShipperMatchCommandPublisher(ShipperMatchCommandInfraProducer infraProducer)
        {
            _infraProducer = infraProducer;
        }

        public async Task PublishAsync(ShipperMatchCommand command)
        {
            var establishmentAddressHash = RedisGeoHashLocationHelper.GetLocationGeoHash(
                command.EstablishmentLocation.Latitude,
                command.EstablishmentLocation.Longitude);

            var model = new GeoHashShipperMatchCommand
            {
                ShippingOrderId = command.ShippingOrderId,
                ShippingOrderCreatedAt = command.ShippingOrderCreatedAt,
                EstablishmentLocation = new GeoHashLocation
                {
                    Latitude = command.EstablishmentLocation.Latitude,
                    Longitude = command.EstablishmentLocation.Longitude,
                    GeoHash = establishmentAddressHash,
                },
                SearchArea = establishmentAddressHash,
                AreaIndex = 0,
            };
            await _infraProducer.PublishAsync(model);
        }
    }
}
