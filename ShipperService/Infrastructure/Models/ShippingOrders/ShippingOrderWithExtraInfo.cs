using hipperService.Infrastructure.Models;
using ShipperService.Domain.Entities;
using System.Collections.Generic;

namespace ShipperService.Infrastructure.Models.ShippingOrders
{
    public class ShippingOrderWithExtraInfo : ShippingOrder
    {
        public IReadOnlyCollection<CompactDomainEvent> DomainEvents { get; set; }
    }
}
