using Base.Domain;
using ShipperService.Domain.Entities;
using System.Threading.Tasks;

namespace ShipperService.Application.Repositories
{
    public interface IShippingOrderRepository : IRepository<ShippingOrder, long>
    {
        public Task<ShippingOrder> GetCurrentAssignedShippingOrder(long shipperId);
    }
}
