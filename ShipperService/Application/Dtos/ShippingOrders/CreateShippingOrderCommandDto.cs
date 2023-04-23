using System;

namespace ShipperService.Application.Dtos.ShippingOrders
{
    public class CreateShippingOrderCommandDto
    {
        public long OrderId { get; set; }
        public decimal FromLatitude { get; set; }
        public decimal ToLatitude { get; set; }
        public decimal FromLongitude { get; set; }
        public decimal ToLongitude { get; set; }
    }
}
