using MilesCarRental.Domain.Entities;
using MilesCarRental.Domain.Exceptions;
using MilesCarRental.Domain.Ports;

namespace MilesCarRental.Domain.Services;

/// <summary>
/// Servicio de dominio que proporciona funcionalidades para manejar y aplicar criterios de mercado a los vehículos.
/// Este servicio utiliza un repositorio de criterios de mercado para recuperar los criterios específicos
/// y luego los aplica a los vehículos para determinar si cumplen con dichos criterios.
/// </summary>

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

    // Recupera los criterios de mercado basados en un identificador específico.
    public async Task<string> GetMarketCriteriaByIdAsync(Guid marketCriteriaId)
    {
        if (marketCriteriaId == Guid.Empty)
        {
            throw new CoreBusinessException(_resourceManager.MarketCriteriaIdNotValid);
        }

        return await _marketCriteriaRepository.GetCriteriaByIdAsync(marketCriteriaId);
    }

    // Aplica los criterios de mercado a un vehículo específico para determinar si cumple con los criterios.
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
