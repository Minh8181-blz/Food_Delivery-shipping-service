using AutoMapper;
using Base.Application;
using MediatR;
using ShipperService.Application.Dtos.ShippingOrders;
using ShipperService.Application.EventPublishers;
using ShipperService.Application.Repositories;
using ShipperService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShipperService.Application.Commands.CreateAcceptedShippingOrder
{
    public class CreateAcceptedShippingOrderCommandHandler
        : IRequestHandler<CreateAcceptedShippingOrderCommand, ShippingOrderCreateResultDto>
    {
        private readonly IShippingOrderRepository _shippingOrderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IShippingOrderEventPublisher _shippingOrderEventPublisher;

        public CreateAcceptedShippingOrderCommandHandler(
            IShippingOrderRepository shippingOrderRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IShippingOrderEventPublisher shippingOrderEventPublisher)
        {
            _shippingOrderRepository = shippingOrderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _shippingOrderEventPublisher = shippingOrderEventPublisher;
        }

        public async Task<ShippingOrderCreateResultDto> Handle(
            CreateAcceptedShippingOrderCommand request,
            CancellationToken cancellationToken)
        {
            var result = new ShippingOrderCreateResultDto();
            var commandDto = request.CommandDto;

            if (commandDto == null ||commandDto.OrderId == 0)
            {
                result.Code = "bad_command";
                result.Message = "Invalid order data";
                return result;
            }

            var order = ShippingOrder.CreatePlacedShippingOrder(
                commandDto.OrderId,
                commandDto.FromLatitude,
                commandDto.FromLongitude,
                commandDto.ToLatitude,
                commandDto.ToLongitude);
            await _shippingOrderRepository.AddAsync(order);
            await _unitOfWork.SaveEntitiesAsync();

            // handle integration event
            _ = Task.Run(async () => {
                await _shippingOrderEventPublisher.PublishAsync(order);
            });

            result.Succeeded = true;
            result.Code = "created";
            result.ShippingOrder = _mapper.Map<ShippingOrderDto>(order);

            return result;
        }
    }
}
