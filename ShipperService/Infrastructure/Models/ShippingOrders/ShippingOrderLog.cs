using System;
using System.ComponentModel.DataAnnotations;

namespace ShipperService.Infrastructure.Models.ShippingOrders
{
    public class ShippingOrderLog
    {
        public long Id { get; set; }
        public long ShippingOrderId { get; set; }
        public long OrderId { get; set; }
        public string Data { get; set; }
        public DateTime CreatedAt { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
