using MediatR;
using ShipperService.Application.Dtos.ShipperSessions;

namespace ShipperService.Application.Commands.AddShipperSession
{
    public class AddShipperSessionCommand : IRequest<ShipperSessionUpdateResultDto>
    {
        public AddShipperSessionCommand(ShipperSessionUpdateCommandDto dto)
        {
            CommandDto = dto;
        }

        public ShipperSessionUpdateCommandDto CommandDto { get; set; }
    }
}
