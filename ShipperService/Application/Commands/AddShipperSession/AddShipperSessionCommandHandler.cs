using AutoMapper;
using Base.Application;
using MediatR;
using ShipperService.Application.Dtos.ShipperSessions;
using ShipperService.Application.EventPublishers;
using ShipperService.Application.Repositories;
using ShipperService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ShipperService.Application.Commands.AddShipperSession
{
    public class AddShipperSessionCommandHandler
        : IRequestHandler<AddShipperSessionCommand, ShipperSessionUpdateResultDto>
    {
        private readonly IShipperSessionRepository _shippperSessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IShipperSessionPublisher _shipperSessionPublisher;

        public AddShipperSessionCommandHandler(
            IShipperSessionRepository shippperSessionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IShipperSessionPublisher shipperSessionPublisher)
        {
            _shippperSessionRepository = shippperSessionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _shipperSessionPublisher = shipperSessionPublisher;
        }

        public async Task<ShipperSessionUpdateResultDto> Handle(AddShipperSessionCommand request, CancellationToken cancellationToken)
        {
            var result = new ShipperSessionUpdateResultDto();
            var commandDto = request.CommandDto;

            if (commandDto == null || commandDto.ShipperId == 0)
            {
                result.Code = "bad_command";
                result.Message = "Invalid order data";
                return result;
            }

            var existingSession = await _shippperSessionRepository.GetLatestActiveSessionByShipperId(commandDto.ShipperId);
            if (existingSession != null)
            {
                result.Code = "active_session_already_exists";
                result.Message = "Active session already exists";
                return result;
            }

            var session = ShipperSession.StartNewSession(commandDto.ShipperId);
            await _shippperSessionRepository.AddAsync(session);
            await _unitOfWork.SaveEntitiesAsync();

            // handle integration event
            _ = Task.Run(async () => {
                await _shipperSessionPublisher.PublishAsync(session);
            });

            result.Succeeded = true;
            result.Code = "created";
            result.ShipperSession = _mapper.Map<ShipperSessionUpdateResultDto>(session);

            return result;
        }
    }
}
