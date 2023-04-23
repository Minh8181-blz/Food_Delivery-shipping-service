using Base.Domain;
using ShipperService.Domain.Enums;
using System;

namespace ShipperService.Domain.Entities
{
    public class ShipperSession : Entity<long>, IAggregateRoot
    {
        public static ShipperSession StartNewSession(long shipperId)
        {
            var session = new ShipperSession
            {
                ShipperId = shipperId,
                Status = ShipperSessionStatus.ACTIVE,
                StartedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
            };
            return session;
        }

        public void End()
        {
            Status = ShipperSessionStatus.ENDED;
            EndedAt = DateTime.UtcNow;
            SetAsModified();
        }

        public long ShipperId { get; private set; }
        public ShipperSessionStatus Status { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? EndedAt { get; private set; }
    }
}
