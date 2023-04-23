using ShipperService.Utils;
using System;

namespace ShipperService.Application.Dtos.ShipperAssignment
{
    public class ShipperMatchCommand
    {
        public long ShippingOrderId { get; set; }
        public DateTime ShippingOrderCreatedAt { get; set; }
        public GeoLocation EstablishmentLocation { get; set; }
    }
}
