using Base.Application.Models;

namespace ShipperService.Application.Dtos.ShipperAssignment
{
    public class ShipperAssignmentResultDto : CommandResultModel
    {
        public long ShipperId { get; set; }
    }
}
