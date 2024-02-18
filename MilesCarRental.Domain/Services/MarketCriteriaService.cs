using MilesCarRental.Domain.Entities;
using MilesCarRental.Domain.Exceptions;
using MilesCarRental.Domain.Ports;

namespace MilesCarRental.Domain.Services;

[DomainService]
public class MarketCriteriaService : IMarketCriteriaService
{
    private readonly IMarketCriteriaRepository _marketCriteriaRepository;
    private readonly ILogMessageService _resourceManager;

    public MarketCriteriaService(IMarketCriteriaRepository marketCriteriaRepository, ILogMessageService resourceManager)
    {
        _marketCriteriaRepository = marketCriteriaRepository ?? throw new ArgumentNullException(nameof(marketCriteriaRepository));
        _resourceManager = resourceManager;
    }

    public async Task<string> GetMarketCriteriaByIdAsync(Guid marketCriteriaId)
    {
        if (marketCriteriaId == Guid.Empty)
        {
            throw new CoreBusinessException(_resourceManager.MarketCriteriaIdNotValid);
        }

        return await _marketCriteriaRepository.GetCriteriaByIdAsync(marketCriteriaId);
    }

    public bool ApplyMarketCriteria(Vehicle vehicle, string marketCriteria)
    {
        if (vehicle == null)
        {
            throw new CoreBusinessException(_resourceManager.VehicleCannotBeNull);
        }


        return marketCriteria.Split(',').Any(criteria =>
            criteria.Equals(_resourceManager.TypeOneVehicle, StringComparison.OrdinalIgnoreCase) && vehicle.Type == _resourceManager.TypeOneVehicle ||
            criteria.Equals(_resourceManager.TypeTwoVehicle, StringComparison.OrdinalIgnoreCase) && vehicle.Type == _resourceManager.TypeTwoVehicle ||
            criteria.StartsWith(_resourceManager.PriceUnder, StringComparison.OrdinalIgnoreCase) && decimal.TryParse(criteria.Replace(_resourceManager.PriceUnder, ""), 
            out var maxPrice) && vehicle.PricePerDay < maxPrice
        );
    }

}
