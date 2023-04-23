using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipperService.Application.Commands.AddShipperSession;
using ShipperService.Application.Dtos.ShipperSessions;
using ShipperService.Presentation.Models.Location;
using System.Threading.Tasks;

namespace ShipperService.Presentation.Controllers
{
    [Route("api/shipper/location")]
    [ApiController]
    [Authorize(Roles = "Shipper")]
    public class ShipperLocationController : ControllerCustomBase
    {
        private readonly IMediator _mediator;

        public ShipperLocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        public async Task UpdateLocation([FromBody] LocationUpdateVm model)
        {
            var command = new UpdateShipperLocationCommand(GetCurrentUserId().Value, model.Latitude, model.Longitude);
            await _mediator.Send(command);
            return;
        }
    }
}
