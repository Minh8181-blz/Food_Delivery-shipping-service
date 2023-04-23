using ShipperService.Infrastructure.Models.ShippingOrders;
using System.Threading.Tasks;

namespace ShipperService.Infrastructure.Daos.Interfaces
{
    public interface IShippingOrderLogDao
    {
        public ShippingOrderLog Add(ShippingOrderLog shippingOrderLog);
        public Task<ShippingOrderLog> AddAsync(ShippingOrderLog shippingOrderLog);
    }
}
