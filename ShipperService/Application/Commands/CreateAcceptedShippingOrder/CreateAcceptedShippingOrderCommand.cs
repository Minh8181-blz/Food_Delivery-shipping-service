using MediatR;
using ShipperService.Application.Dtos.ShippingOrders;

namespace ShipperService.Application.Commands.CreateAcceptedShippingOrder
{
    public class CreateAcceptedShippingOrderCommand : IRequest<ShippingOrderCreateResultDto>
    {
        public CreateAcceptedShippingOrderCommand(CreateShippingOrderCommandDto dto)
        {
            CommandDto = dto;
        }

        public CreateShippingOrderCommandDto CommandDto { get; set; }
    }
}
