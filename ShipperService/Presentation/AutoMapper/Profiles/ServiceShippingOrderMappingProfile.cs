using AutoMapper;
using ShipperService.Application.Dtos.ShippingOrders;
using ShipperService.Presentation.Models.ShippingOrders;

namespace ShipperService.Presentation.AutoMapper.Profiles
{
    public class ServiceShippingOrderMappingProfile : Profile
    {
        public ServiceShippingOrderMappingProfile()
        {
            CreateMap<CreateShippingOrderVm, CreateShippingOrderCommandDto>();
        }
    }
}
