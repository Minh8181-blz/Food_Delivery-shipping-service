using MediatR;
using ShipperService.Application.Dtos.ShipperAssignment;

namespace ShipperService.Application.Commands.AssignShipperToOrder
{
    public class AssignShipperToOrderCommand : IRequest<ShipperAssignmentResultDto>
    {
        public long ShipperId { get; set; }
        public long ShippingOrderId { get; set; }
    }
}
