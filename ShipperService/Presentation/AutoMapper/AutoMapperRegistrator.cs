using Microsoft.Extensions.DependencyInjection;
using ShipperService.Application.AutoMapper.Profiles;
using ShipperService.Infrastructure.AutoMapper.Profiles;
using ShipperService.Presentation.AutoMapper.Profiles;

namespace ShipperService.Presentation.AutoMapper
{
    public static class AutoMapperRegistrator
    {
        public static void AddAutoMapperService(this IServiceCollection services)
        {

            services.AddByMethod1();
        }

        private static void AddByMethod1(this IServiceCollection services)
        {
            // param là assembly nên điền class gì cũng được, miễn là nó thuộc assembly cần dùng
            services.AddAutoMapper(
                // assembly tầng service
                typeof(ServiceShippingOrderMappingProfile).Assembly,
                // assembly tầng app
                typeof(ShippingOrderMappingProfile).Assembly,
                // assembly tầng infra
                typeof(InfrastructureShippingOrderMappingProfile).Assembly
            );
        }
    }
}
