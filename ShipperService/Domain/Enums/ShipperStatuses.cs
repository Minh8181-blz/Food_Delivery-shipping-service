namespace ShipperService.Domain.Enums
{
    public enum ShipperSessionStatus
    {
        ENDED,
        ACTIVE
    }

    public enum ShippingOrderStatus
    {
        PLACED,
        ACCEPTED,
        DECLINED,
        PACKING,
        DELIVERING,
        DELIVERED,
        CANCELED,
    }
}
