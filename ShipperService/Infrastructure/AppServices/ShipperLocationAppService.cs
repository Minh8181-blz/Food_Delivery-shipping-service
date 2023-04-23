using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShipperService.Application.Services;
using ShipperService.Infrastructure.Helpers;
using ShipperService.Utils;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace ShipperService.Infrastructure.AppServices
{
    public class ShipperLocationAppService : IShipperLocationAppService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<ShipperLocationAppService> _logger;

        public ShipperLocationAppService(IConnectionMultiplexer redis, ILogger<ShipperLocationAppService> logger)
        {
            _redis = redis;
            _logger = logger;
        }

        public async Task RemoveShipperLocationAsync(long shipperId)
        {
            try
            {
                var db = _redis.GetDatabase();
                string shipperLocationKey = RedisGeoHashLocationHelper.GetRedisShipperLocationKey(shipperId);
                var currentLocationValue = await db.StringGetAsync(shipperLocationKey);
                if (currentLocationValue.HasValue)
                {
                    var currentLocation = JsonConvert.DeserializeObject<GeoHashLocation>(currentLocationValue);
                    var areaShippersKey = RedisGeoHashLocationHelper.GetRedisAreaShippersKey(currentLocation.GeoHash);
                    var success = await db.SetRemoveAsync(areaShippersKey, shipperId);
                    if (success)
                    {
                        await db.KeyDeleteAsync(shipperLocationKey);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task UpdateShipperLocationAsync(long shipperId, decimal latitude, decimal longitude)
        {
            try
            {
                var db = _redis.GetDatabase();
                var newArea = RedisGeoHashLocationHelper.GetLocationGeoHash(latitude, longitude);
                string shipperLocationKey = RedisGeoHashLocationHelper.GetRedisShipperLocationKey(shipperId);
                var enterNewArea = true;
                var currentLocationValue = await db.StringGetAsync(shipperLocationKey);
                if (currentLocationValue.HasValue)
                {
                    var currentLocation = JsonConvert.DeserializeObject<GeoHashLocation>(currentLocationValue);
                    if (currentLocation.GeoHash != newArea)
                    {
                        var currentAreaShippersKey = RedisGeoHashLocationHelper.GetRedisAreaShippersKey(currentLocation.GeoHash);
                        await db.SetRemoveAsync(currentAreaShippersKey, shipperId);
                    }
                    else
                    {
                        enterNewArea = false;
                    }
                }
                
                var location = new GeoHashLocation
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    GeoHash = newArea,
                };
                var locationStr = JsonConvert.SerializeObject(location);
                await db.StringSetAsync(shipperLocationKey, locationStr);

                if (enterNewArea)
                {
                    var newAreaShippersKey = RedisGeoHashLocationHelper.GetRedisAreaShippersKey(newArea);
                    await db.SetAddAsync(newAreaShippersKey, shipperId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
