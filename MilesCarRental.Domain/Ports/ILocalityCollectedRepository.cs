using MilesCarRental.Domain.Entities;

namespace MilesCarRental.Domain.Ports;

public interface ILocalityCollectedRepository
{
    Task<IEnumerable<Location>> FindLocationsByCityOrPlaceNameAsync(string searchQuery);
    Task<IEnumerable<Vehicle>> FindAvailableVehiclesAsync(Guid pickupLocationId, Guid dropOffLocationId, DateTime startDate, DateTime endDate);
}
