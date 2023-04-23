using MediatR;
using ShipperService.Domain.Entities;

namespace ShipperService.Application.Commands.FindShipper
{
    public class FindShipperCommand : IRequest<bool>
    {
        public ShippingOrder ShippingOrder { get; set; }
    }
}
