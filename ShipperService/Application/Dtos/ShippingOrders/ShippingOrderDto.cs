using ShipperService.Domain.Enums;
using System;

namespace ShipperService.Application.Dtos.ShippingOrders
{
    public class ShippingOrderDto
    {
        public long OrderId { get; set; }
        public ShippingOrderStatus Status { get; set; }
        public long? AssignedShipperId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
