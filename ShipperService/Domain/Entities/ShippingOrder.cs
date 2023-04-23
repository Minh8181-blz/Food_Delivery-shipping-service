using Base.Domain;
using ShipperService.Domain.DomainEvents.ShippingOrders;
using ShipperService.Domain.Enums;
using ShipperService.Domain.Exceptions;
using System;

namespace ShipperService.Domain.Entities
{
    public class ShippingOrder : Entity<long>, IAggregateRoot
    {
        public static ShippingOrder CreatePlacedShippingOrder(
            long orderId,
            decimal fromLatitude,
            decimal fromLongitude,
            decimal toLatitude,
            decimal toLongitude)
        {
            var shippingOrder = new ShippingOrder
            {
                OrderId = orderId,
                FromLatitude = fromLatitude,
                FromLongitude = fromLongitude,
                ToLatitude = toLatitude,
                ToLongitude = toLongitude,
                Status = ShippingOrderStatus.PLACED,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
            };

            shippingOrder.AddDomainEvent(new ShippingOrderPlacedDomainEvent(shippingOrder));

            return shippingOrder;
        }

        public void AssignShipper(long shipperId)
        {
            if (Status != ShippingOrderStatus.PLACED)
            {
                throw new UnsuitableStatusDomainException
                {
                    EntityType = GetType().Name,
                    EntityStatus = Status.ToString(),
                };
            }
            AssignedShipperId = shipperId;
            SetAsModified();
        }

        public decimal FromLatitude { get; private set; }
        public decimal FromLongitude { get; private set; }
        public decimal ToLatitude { get; private set; }
        public decimal ToLongitude { get; private set; }
        public long OrderId { get; private set; }
        public ShippingOrderStatus Status { get; private set; }
        public long? AssignedShipperId { get; private set; }
    }
}
