using AutoMapper;
using ShipperService.Application.Dtos.ShipperSessions;
using ShipperService.Application.Dtos.ShippingOrders;
using ShipperService.Domain.Entities;

namespace ShipperService.Application.AutoMapper.Profiles
{
    public class ShippingOrderMappingProfile : Profile
    {
        public ShippingOrderMappingProfile()
        {
            CreateMap<ShippingOrder, ShippingOrderDto>();
            CreateMap<ShipperSession, ShipperSessionUpdateResultDto>();
        }
    }
}
