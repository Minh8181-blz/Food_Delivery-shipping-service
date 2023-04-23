using AutoMapper;
using ShipperService.Domain.Entities;
using ShipperService.Infrastructure.Helpers;
using ShipperService.Infrastructure.Models.ShippingOrders;

namespace ShipperService.Infrastructure.AutoMapper.Profiles
{
    public class InfrastructureShippingOrderMappingProfile : Profile
    {
        public InfrastructureShippingOrderMappingProfile()
        {
            CreateMap<ShippingOrder, ShippingOrderWithExtraInfo>()
                .ForMember(
                    d => d.DomainEvents,
                    e => e.MapFrom(o => ShippingOrderLogHelper.GenerateCompactDomainEvents(o.GetDomainEvents())
                ));
        }
    }
}
