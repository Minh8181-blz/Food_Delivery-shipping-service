using Microsoft.Extensions.DependencyInjection;
using ShipperService.Infrastructure.ShipperMatcherExecutor;

namespace ShipperService.Infrastructure.DependencyRegistrations
{
    public static class InfrastructureServiceRegistrator
    {
        public static void AddInfrastructureLayerServices(this IServiceCollection services)
        {
            services.AddTransient<ShipperMatcher>();
            services.AddSingleton<ShipperMatchCommandInfraProducer>();
        }
    }
}
