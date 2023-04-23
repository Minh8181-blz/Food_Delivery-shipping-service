using System.Threading.Tasks;

namespace ShipperService.Application.Services
{
    public interface IShipperLocationAppService
    {
        Task UpdateShipperLocationAsync(long shipperId, decimal latitude, decimal longitude);
        Task RemoveShipperLocationAsync(long shipperId);
    }
}
