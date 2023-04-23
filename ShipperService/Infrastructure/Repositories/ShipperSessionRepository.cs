using Base.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShipperService.Application.Repositories;
using ShipperService.Domain.Entities;
using ShipperService.Domain.Enums;
using ShipperService.Infrastructure.Database;
using System.Linq;
using System.Threading.Tasks;

namespace ShipperService.Infrastructure.Repositories
{
    public class ShipperSessionRepository : RepositoryBase<ShipperDbContext, ShipperSession, long>, IShipperSessionRepository
    {
        public ShipperSessionRepository(ShipperDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ShipperSession> GetLatestActiveSessionByShipperId(long shipperId)
        {
            return await _context.ShipperSessions
                .FirstOrDefaultAsync(s => s.ShipperId == shipperId && s.Status == ShipperSessionStatus.ACTIVE);
        }
    }
}
