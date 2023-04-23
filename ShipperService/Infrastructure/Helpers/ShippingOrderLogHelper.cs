using Base.Domain;
using hipperService.Infrastructure.Models;
using ShipperService.Domain.DomainEvents.ShippingOrders;
using System.Collections.Generic;
using System.Linq;

namespace ShipperService.Infrastructure.Helpers
{
    public class ShippingOrderLogHelper
    {
        public static IEnumerable<CompactDomainEvent> GenerateCompactDomainEvents(IEnumerable<DomainEvent> orderDomainEvents)
        {
            return orderDomainEvents?.Select(domainEvent =>
            {
                return new CompactDomainEvent
                {
                    Name = (domainEvent as BaseShippingOrderDomainEvent).Name,
                };
            });
        }
    }
}
