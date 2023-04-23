using MediatR;

namespace ShipperService.Application.Commands.AddShipperSession
{
    public class UpdateShipperLocationCommand : IRequest<bool>
    {
        public UpdateShipperLocationCommand(long shipperId, decimal latitude, decimal longitude)
        {
            ShipperId = shipperId;
            Latitude = latitude;
            Longitude = longitude;
        }

        public long ShipperId { get; }
        public decimal Latitude { get; }
        public decimal Longitude { get; }
    }
}
