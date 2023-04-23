using Geohash;

namespace ShipperService.Infrastructure.Helpers
{
    public static class RedisGeoHashLocationHelper
    {
        public const int HashPrecisionLevel = 6;

        private const string ShipperLocationKeyPattern = "shipper_location_{0}";
        private const string GeoHashAreaShippersPattern = "area_shippers_{0}";
        private const string ShipperCurrentOrderIdPattern = "shipper_current_order_{0}";
        private static readonly Geohasher _hasher = new Geohasher();

        public static string GetRedisShipperLocationKey(long shipperId)
        {
            return string.Format(ShipperLocationKeyPattern, shipperId);
        }

        public static string GetRedisAreaShippersKey(string geoHashArea)
        {
            return string.Format(GeoHashAreaShippersPattern, geoHashArea);
        }

        public static string GetLocationGeoHash(decimal lat, decimal lng, int precisionLevel = HashPrecisionLevel)
        {
            return _hasher.Encode(decimal.ToDouble(lat), decimal.ToDouble(lng), precisionLevel);
        }

        public static string GetRedisShipperCurrentOrderKey(long shipperId)
        {
            return string.Format(ShipperCurrentOrderIdPattern, shipperId);
        }

        /// <summary>
        /// 8 | 1 | 2
        /// ---------
        /// 7 | 0 | 3
        /// ---------
        /// 6 | 5 | 4
        /// </summary>
        /// <param name="currentAreaHash"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetNextAreaForShipperScan(string currentAreaHash, int index)
        {
            var currentArea = _hasher.GetNeighbors(currentAreaHash);
            switch (index)
            {
                case 0:
                    return currentArea[Direction.North];
                case 1:
                    return currentArea[Direction.East];
                case 2:
                    return currentArea[Direction.South];
                case 3:
                    return currentArea[Direction.South];
                case 4:
                    return currentArea[Direction.West];
                case 5:
                    return currentArea[Direction.West];
                case 6:
                    return currentArea[Direction.North];
                case 7:
                    return currentArea[Direction.North];
                case 8:
                    return currentArea[Direction.SouthEast];
                default:
                    return currentAreaHash;
            }
        }
    }
}
