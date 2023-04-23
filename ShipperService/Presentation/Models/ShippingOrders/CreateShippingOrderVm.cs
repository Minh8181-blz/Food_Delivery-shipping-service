using System.ComponentModel.DataAnnotations;

namespace ShipperService.Presentation.Models.ShippingOrders
{
    public class CreateShippingOrderVm
    {
        [Range(1, long.MaxValue)]
        public long OrderId { get; set; }
    }
}
