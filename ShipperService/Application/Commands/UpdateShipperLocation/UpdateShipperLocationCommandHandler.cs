using MediatR;
using ShipperService.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace ShipperService.Application.Commands.AddShipperSession
{
    public class UpdateShipperLocationCommandHandler
        : IRequestHandler<UpdateShipperLocationCommand, bool>
    {
        private readonly IShipperLocationAppService _shipperLocationAppService;        

        public UpdateShipperLocationCommandHandler(
            IShipperLocationAppService shipperLocationAppService)
        {
           _shipperLocationAppService = shipperLocationAppService;
        }

        public async Task<bool> Handle(UpdateShipperLocationCommand request, CancellationToken cancellationToken)
        {
            await _shipperLocationAppService.UpdateShipperLocationAsync(request.ShipperId, request.Latitude, request.Longitude);
            return true;
        }
    }
}
