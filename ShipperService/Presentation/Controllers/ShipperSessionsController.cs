using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipperService.Application.Commands.AddShipperSession;
using ShipperService.Application.Dtos.ShipperSessions;
using System.Threading.Tasks;

namespace ShipperService.Presentation.Controllers
{
    [Route("api/shipper/sessions")]
    [ApiController]
    [Authorize(Roles = "Shipper")]
    public class ShipperSessionsController : ControllerCustomBase
    {
        private readonly IMediator _mediator;

        public ShipperSessionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ShipperSessionUpdateResultDto> StartNewSession()
        {
            var command = new AddShipperSessionCommand(new ShipperSessionUpdateCommandDto
            {
                ShipperId = GetCurrentUserId().Value,
            });

            var result = await _mediator.Send(command);
            if (!result.Succeeded)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            return result;
        }
    }
}
