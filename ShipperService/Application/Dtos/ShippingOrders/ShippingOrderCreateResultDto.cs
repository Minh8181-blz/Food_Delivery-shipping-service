using Base.Application.Models;

namespace ShipperService.Application.Dtos.ShippingOrders
{
    public class ShippingOrderCreateResultDto : CommandResultModel
    {
        public ShippingOrderDto ShippingOrder { get; set; }
    }
}
