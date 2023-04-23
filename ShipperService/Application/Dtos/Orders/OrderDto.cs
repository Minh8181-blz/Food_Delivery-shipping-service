using ShipperService.Application.Enums;
using System;

namespace ShipperService.Application.Dtos.Orders
{
    public class OrderDto
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public long EstablishmentId { get; set; }
        public decimal FromLatitude { get; set; }
        public decimal FromLongitude { get; set; }
        public string FromAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string Note { get; set; }
        public OrderPlacementStatus PlacementStatus { get; set; }
        public DateTime? ExpectedDeliveryAt { get; set; }
        public OrderAcceptanceStatus AcceptanceStatus { get; set; }
        public OrderPaymentStatus PaymentStatus { get; set; }
        public OrderShippingStatus ShippingStatus { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public decimal ShippingLatitude { get; set; }
        public decimal ShippingLongitude { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
