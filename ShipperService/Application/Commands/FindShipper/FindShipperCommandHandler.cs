using MediatR;
using ShipperService.Application.Dtos.ShipperAssignment;
using ShipperService.Application.EventPublishers;
using ShipperService.Utils;
using System.Threading;
using System.Threading.Tasks;

namespace ShipperService.Application.Commands.FindShipper
{
    public class FindShipperCommandHandler : IRequestHandler<FindShipperCommand, bool>
    {
        private readonly IShipperMatchCommandPublisher _shipperMatchCommandPublisher;

        public FindShipperCommandHandler(IShipperMatchCommandPublisher shipperMatchCommandPublisher)
        {
            _shipperMatchCommandPublisher = shipperMatchCommandPublisher;
        }

        public async Task<bool> Handle(FindShipperCommand request, CancellationToken cancellationToken)
        {
            await _shipperMatchCommandPublisher.PublishAsync(new ShipperMatchCommand
            {
                ShippingOrderId = request.ShippingOrder.Id,
                ShippingOrderCreatedAt = request.ShippingOrder.CreatedAt,
                EstablishmentLocation = new GeoLocation
                {
                    Latitude = request.ShippingOrder.FromLatitude,
                    Longitude = request.ShippingOrder.FromLongitude,
                }
            });
            return true;
        }
    }
}
