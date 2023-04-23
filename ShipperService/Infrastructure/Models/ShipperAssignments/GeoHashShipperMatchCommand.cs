using ShipperService.Utils;
using System;

namespace ShipperService.Infrastructure.Models.ShipperAssignments
{
    public class GeoHashShipperMatchCommand
    {
        public long ShippingOrderId { get; set; }
        public DateTime ShippingOrderCreatedAt { get; set; }
        public GeoHashLocation EstablishmentLocation { get; set; }
        public string SearchArea { get; set; }
        public int AreaIndex { get; set; }
    }
}
