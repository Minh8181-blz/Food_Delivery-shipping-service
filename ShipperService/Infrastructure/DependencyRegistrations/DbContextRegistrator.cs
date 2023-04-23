using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShipperService.Infrastructure.Database;

namespace ShipperService.Infrastructure.DependencyRegistrations
{
    public static class DbContextRegistrator
    {
        public static void AddSqlServerDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ShipperDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }
    }
}
