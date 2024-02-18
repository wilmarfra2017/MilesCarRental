using MilesCarRental.Domain.Entities;

namespace MilesCarRental.Domain.Ports;

public interface IMarketCriteriaService
{
    bool ApplyMarketCriteria(Vehicle vehicle, string marketCriteria);
    Task<string> GetMarketCriteriaByIdAsync(Guid marketCriteriaId);
}
