using ShipperService.Infrastructure.Daos.Interfaces;
using ShipperService.Infrastructure.Database;
using ShipperService.Infrastructure.Models.ShippingOrders;
using System.Threading.Tasks;

namespace ShipperService.Infrastructure.Daos
{
    public class ShippingOrderLogDao : IShippingOrderLogDao
    {
        private readonly ShipperDbContext _dbContext;

        public ShippingOrderLogDao(
            ShipperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ShippingOrderLog Add(ShippingOrderLog shippingOrderLog)
        {
            return _dbContext.Add(shippingOrderLog).Entity;
        }

        public async Task<ShippingOrderLog> AddAsync(ShippingOrderLog shippingOrderLog)
        {
            return (await _dbContext.AddAsync(shippingOrderLog)).Entity;
        }
    }
}
