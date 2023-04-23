using Base.Domain;
using ShipperService.Domain.Entities;
using System.Threading.Tasks;

namespace ShipperService.Application.Repositories
{
    public interface IShipperSessionRepository : IRepository<ShipperSession, long>
    {
        Task<ShipperSession> GetLatestActiveSessionByShipperId(long shipperId);
    }
}
