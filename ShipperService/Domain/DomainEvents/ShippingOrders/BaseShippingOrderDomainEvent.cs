using Base.Domain;
using ShipperService.Domain.Entities;

namespace ShipperService.Domain.DomainEvents.ShippingOrders
{
    public class BaseShippingOrderDomainEvent : DomainEvent
    {
        public ShippingOrder ShippingOrder { get; protected set; }
        public string Name { get; protected set; }
    }
}
