using ShipperService.Domain.Entities;
using System.Threading.Tasks;

namespace ShipperService.Application.EventPublishers
{
    public interface IShippingOrderEventPublisher
    {
        Task PublishAsync(ShippingOrder shippingOrder);
    }
}
