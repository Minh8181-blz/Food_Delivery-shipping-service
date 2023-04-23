using ShipperService.Domain.Entities;
using System.Threading.Tasks;

namespace ShipperService.Application.EventPublishers
{
    public interface IShipperSessionPublisher
    {
        Task PublishAsync(ShipperSession shippingOrder);
    }
}
