using ShipperService.Domain.Enums;
using System;

namespace ShipperService.Application.Dtos.ShipperSessions
{
    public class ShipperSessionDto
    {
        public long Id { get; set; }
        public long ShipperId { get; private set; }
        public ShipperSessionStatus Status { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? EndedAt { get; private set; }
    }
}
