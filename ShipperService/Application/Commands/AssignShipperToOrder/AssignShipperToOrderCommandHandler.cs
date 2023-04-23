using Base.Application;
using MediatR;
using ShipperService.Application.Dtos.ShipperAssignment;
using ShipperService.Application.EventPublishers;
using ShipperService.Application.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace ShipperService.Application.Commands.AssignShipperToOrder
{
    public class AssignShipperToOrderCommandHandler
        : IRequestHandler<AssignShipperToOrderCommand, ShipperAssignmentResultDto>
    {
        private readonly IShippingOrderRepository _shippingOrderRepository;
        private readonly IShippingOrderEventPublisher _shippingOrderEventPublisher;
        private readonly IUnitOfWork _unitOfWork;

        public AssignShipperToOrderCommandHandler(
            IShippingOrderRepository shippingOrderRepository,
            IShippingOrderEventPublisher shippingOrderEventPublisher,
            IUnitOfWork unitOfWork)
        {
            _shippingOrderRepository = shippingOrderRepository;
            _shippingOrderEventPublisher = shippingOrderEventPublisher;
            _unitOfWork = unitOfWork;
        }

        public async Task<ShipperAssignmentResultDto> Handle(AssignShipperToOrderCommand request, CancellationToken cancellationToken)
        {
            var result = new ShipperAssignmentResultDto();
            var currentAssignedOrder = await _shippingOrderRepository.GetCurrentAssignedShippingOrder(request.ShipperId);
            if (currentAssignedOrder != null)
            {
                result.Code = "shipper_not_available";
                result.Message = "Shipper is not available";
                return result;
            }
            var order = await _shippingOrderRepository.GetAsync(request.ShippingOrderId);
            if (order == null)
            {
                result.Code = "order_not_found";
                result.Message = "Order is not found";
                return result;
            }

            order.AssignShipper(request.ShipperId);
            await _unitOfWork.SaveEntitiesAsync();

            // handle integration event
            _ = Task.Run(async () => {
                await _shippingOrderEventPublisher.PublishAsync(order);
            });

            result.Succeeded = true;
            result.ShipperId = order.AssignedShipperId.Value;
            return result;
        }
    }
}
