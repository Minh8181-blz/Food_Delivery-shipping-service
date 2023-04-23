using Base.Domain;

namespace ShipperService.Domain.Exceptions
{
    public class UnsuitableStatusDomainException : DomainException
    {
        public string EntityType { get; set; }
        public string EntityStatus { get; set; }
    }
}
