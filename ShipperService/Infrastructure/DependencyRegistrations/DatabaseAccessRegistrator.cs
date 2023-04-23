using Base.Application;
using Base.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using ShipperService.Application.Repositories;
using ShipperService.Infrastructure.Daos;
using ShipperService.Infrastructure.Daos.Interfaces;
using ShipperService.Infrastructure.Database;
using ShipperService.Infrastructure.Repositories;

namespace ShipperService.Infrastructure.DependencyRegistrations
{
    public static class DatabaseAccessRegistrator
    {
        public static void AddDatabaseAccessServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<ShipperDbContext>>();
            services.AddTransient<IShipperSessionRepository, ShipperSessionRepository>();
            services.AddTransient<IShippingOrderRepository, ShippingOrderRepository>();
            services.AddTransient<IShippingOrderLogDao, ShippingOrderLogDao>();
        }
    }
}
