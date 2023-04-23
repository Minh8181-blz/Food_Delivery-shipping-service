using ShipperService.Application.Dtos.ShipperAssignment;
using System.Threading.Tasks;

namespace ShipperService.Application.EventPublishers
{
    public interface IShipperMatchCommandPublisher
    {
        Task PublishAsync(ShipperMatchCommand shippingOrder);
    }
}
