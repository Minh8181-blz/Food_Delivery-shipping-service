using hipperService.Infrastructure.Models;
using ShipperService.Application.Dtos.Orders;
using System.Collections.Generic;

namespace FoodEstablishment.Infrastructure.Models.Orders
{
    public class OrderWithExtraInfo : OrderDto
    {
        public IReadOnlyCollection<CompactDomainEvent> DomainEvents { get; set; }
    }
}
