using MediatR;
using Microsoft.Extensions.Logging;
using ShipperService.Application.Commands.AssignShipperToOrder;
using ShipperService.Infrastructure.Helpers;
using ShipperService.Infrastructure.Models.ShipperAssignments;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace ShipperService.Infrastructure.ShipperMatcherExecutor
{
    public class ShipperMatcher
    {
        private IMediator _mediator;
        private readonly IConnectionMultiplexer _redis;
        private readonly ShipperMatchCommandInfraProducer _shipperMatchInfraProducer;
        private readonly ILogger<ShipperMatcher> _logger;

        public ShipperMatcher(
            IMediator mediator,
            IConnectionMultiplexer redis,
            ILogger<ShipperMatcher> logger,
            ShipperMatchCommandInfraProducer shipperMatchInfraProducer)
        {
            _mediator = mediator;
            _redis = redis;
            _logger = logger;
            _shipperMatchInfraProducer = shipperMatchInfraProducer;
        }

        public ShipperMatcher BindCustomerMediatorService(IMediator mediator)
        {
            _mediator = mediator;
            return this;
        }

        public async Task FindShipperForShippingOrder(GeoHashShipperMatchCommand command)
        {
            var candidateShipperId = await ScanForShippers(command.SearchArea);
            var succeeded = false;
            if (candidateShipperId.HasValue)
            {
                var assignmentCommand = new AssignShipperToOrderCommand
                {
                    ShipperId = candidateShipperId.Value,
                    ShippingOrderId = command.ShippingOrderId,
                };
                var matchResult = await _mediator.Send(assignmentCommand);
                succeeded = matchResult.Succeeded;
            }

            if (succeeded)
            {
                await MarkShipperAsAssignedWithOrder(candidateShipperId.Value, command.ShippingOrderId);
            }
            // if order created date < 15min, we continue. Otherwise we stop (todo: move this to domain layer)
            else if (DateTime.UtcNow.Subtract(command.ShippingOrderCreatedAt).TotalMinutes < 15)
            {
                command.SearchArea = RedisGeoHashLocationHelper.GetNextAreaForShipperScan(command.SearchArea, command.AreaIndex);
                command.AreaIndex = (command.AreaIndex + 1) % 9;
                await _shipperMatchInfraProducer.PublishAsync(command);
            }
        }

        private async Task<long?> ScanForShippers(string geoHashArea)
        {
            try
            {
                long? shipperId = null;
                var db = _redis.GetDatabase();
                var areaShippersKey = RedisGeoHashLocationHelper.GetRedisAreaShippersKey(geoHashArea);

                var result = db.SetScanAsync(areaShippersKey, pageSize: 1, cursor: 0);
                var enumerator = result.GetAsyncEnumerator();

                while(!shipperId.HasValue && (await enumerator.MoveNextAsync()))
                {
                    var currentShipperId = long.Parse(enumerator.Current);
                    var shipperCurrentOrderRedisKey = RedisGeoHashLocationHelper.GetRedisShipperCurrentOrderKey(currentShipperId);
                    var shipperCurrentOrderValue = await db.StringGetAsync(shipperCurrentOrderRedisKey);
                    if (!shipperCurrentOrderValue.HasValue)
                    {
                        shipperId = currentShipperId;
                    }
                    Console.WriteLine(shipperId);
                };
               
                return shipperId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        private async Task MarkShipperAsAssignedWithOrder(long shipperId, long orderId)
        {
            try
            {
                var db = _redis.GetDatabase();
                var shipperCurrentOrderRedisKey = RedisGeoHashLocationHelper.GetRedisShipperCurrentOrderKey(shipperId);
                await db.StringSetAsync(shipperCurrentOrderRedisKey, orderId.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
