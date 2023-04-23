using MediatR;

namespace ShipperService.Application.Base.Models
{
    public class MediatorNotifcationModel<T> : INotification where T : class
    {
        public T Payload { get; set; }
    }
}
