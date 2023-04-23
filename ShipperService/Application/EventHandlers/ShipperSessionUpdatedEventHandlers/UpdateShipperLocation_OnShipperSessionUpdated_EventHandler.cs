using MediatR;
using ShipperService.Application.Base.Models;
using ShipperService.Application.Services;
using ShipperService.Domain.Entities;
using ShipperService.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace ShipperService.Application.EventHandlers.AppLayer.ShipperSessionAddedEventHandlers
{
    public class UpdateShipperLocation_OnShipperSessionUpdated_EventHandler
        : INotificationHandler<MediatorNotifcationModel<ShipperSession>>
    {
        private readonly IShipperLocationAppService _shipperLocationAppService;

        public UpdateShipperLocation_OnShipperSessionUpdated_EventHandler(
            IShipperLocationAppService shipperLocationAppService
        ) {
            _shipperLocationAppService = shipperLocationAppService;
        }

        public async Task Handle(MediatorNotifcationModel<ShipperSession> notification, CancellationToken cancellationToken)
        {
            var session = notification.Payload;
            if (session == null)
                return;

            if (session.Status == ShipperSessionStatus.ENDED)
            {
                await Task.Run(() => _shipperLocationAppService.RemoveShipperLocationAsync(session.ShipperId));
            }
        }
    }
}
