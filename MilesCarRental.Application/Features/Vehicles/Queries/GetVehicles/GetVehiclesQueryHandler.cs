using MediatR;
using MilesCarRental.Domain.Dtos;
using MilesCarRental.Domain.Entities;
using MilesCarRental.Domain.Ports;

namespace MilesCarRental.Application.Features.Vehicles.Queries.GetVehicles;

/// <summary>
/// Manejador de la consulta GetVehiclesQuery, responsable de implementar la lógica para obtener los vehículos disponibles
/// basados en los criterios especificados en la consulta, como ubicaciones de recogida y entrega, fechas y criterios de mercado.
/// </summary>
public class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesQuery, IEnumerable<VehicleDto>>
{
    private readonly IMarketCriteriaService _marketCriteriaService;
    private readonly ILocalityCollectedRepository _localityCollectedRepository;

    public GetVehiclesQueryHandler(IMarketCriteriaService marketCriteriaService, ILocalityCollectedRepository localityCollectedRepository)
    {
        _marketCriteriaService = marketCriteriaService;
        _localityCollectedRepository = localityCollectedRepository;
    }

    // Procesa la consulta GetVehiclesQuery, aplicando los criterios de búsqueda y filtrado para retornar los vehículos disponibles.
    public async Task<IEnumerable<VehicleDto>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        var marketCriteria = await _marketCriteriaService.GetMarketCriteriaByIdAsync(request.marketId);

        IEnumerable<Vehicle> availableVehicles;
        if (!string.IsNullOrWhiteSpace(request.locationSearchQuery))
        {
            var matchingLocations = await _localityCollectedRepository.FindLocationsByCityOrPlaceNameAsync(request.locationSearchQuery);
            var tasks = matchingLocations.Select(location =>
                _localityCollectedRepository.FindAvailableVehiclesAsync(location.Id, location.Id, request.startDate, request.endDate));

            var vehiclesAtLocations = await Task.WhenAll(tasks);
            availableVehicles = vehiclesAtLocations.SelectMany(vehicles => vehicles);
        }
        else
        {
            availableVehicles = await _localityCollectedRepository.FindAvailableVehiclesAsync(request.pickupLocationId, request.dropOffLocationId, 
                request.startDate, request.endDate);
        }

        var filteredVehicles = availableVehicles
            .Where(vehicle => _marketCriteriaService.ApplyMarketCriteria(vehicle, marketCriteria))
            .Select(vehicle => new VehicleDto(vehicle.Id, vehicle.Brand, vehicle.Model, vehicle.Year, vehicle.Type));

        return filteredVehicles;
    }

}
