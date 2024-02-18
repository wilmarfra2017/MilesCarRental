namespace MilesCarRental.Domain.Ports;

public interface IMarketCriteriaRepository
{
    Task<string> GetCriteriaByIdAsync(Guid marketCriteriaId);
}
