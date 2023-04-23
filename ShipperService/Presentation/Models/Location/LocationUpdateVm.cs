using System.ComponentModel.DataAnnotations;

namespace ShipperService.Presentation.Models.Location
{
    public class LocationUpdateVm
    {
        [Range(-90, 90)]
        public decimal Latitude { get; set; }
        [Range(-180, 180)]
        public decimal Longitude { get; set; }
    }
}
