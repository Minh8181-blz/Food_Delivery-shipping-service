using Base.Application.Models;

namespace ShipperService.Application.Dtos.ShipperSessions
{
    public class ShipperSessionUpdateResultDto : CommandResultModel
    {
        public ShipperSessionUpdateResultDto ShipperSession { get; set; }
    }
}
