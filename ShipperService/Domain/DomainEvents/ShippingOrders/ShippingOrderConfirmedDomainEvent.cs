using ShipperService.Domain.Entities;

namespace ShipperService.Domain.DomainEvents.ShippingOrders
{
    public class ShippingOrderConfirmedDomainEvent : BaseShippingOrderDomainEvent
    {
        public ShippingOrderConfirmedDomainEvent(ShippingOrder shippingOrder)
        {
            Name = "shipping_order_confirmed";
            ShippingOrder = shippingOrder;
        }
    }
}
