using ShipperService.Domain.Entities;

namespace ShipperService.Domain.DomainEvents.ShippingOrders
{
    public class ShippingOrderPlacedDomainEvent : BaseShippingOrderDomainEvent
    {
        public ShippingOrderPlacedDomainEvent(ShippingOrder shippingOrder)
        {
            Name = "shipping_order_placed";
            ShippingOrder = shippingOrder;
        }
    }
}
