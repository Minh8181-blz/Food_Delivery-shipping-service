using MediatR;
using ShipperService.Application.Base.Models;
using ShipperService.Application.EventPublishers;
using ShipperService.Domain.Entities;
using System.Threading.Tasks;

namespace ShipperService.Infrastructure.EventPublishers
{
    public class ShipperSessionPublisher : IShipperSessionPublisher
    {
        private readonly IMediator _mediator;

        public ShipperSessionPublisher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishAsync(ShipperSession session)
        {
            var notiModel = new MediatorNotifcationModel<ShipperSession>
            {
                Payload = session,
            };
            await _mediator.Publish(notiModel);
        }
    }
}
