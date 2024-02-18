using Microsoft.EntityFrameworkCore;
using MilesCarRental.Domain.Entities;
using MilesCarRental.Domain.Ports;
using MilesCarRental.Infrastructure.DataSource;

namespace MilesCarRental.Infrastructure.Adapters;

[Repository]
public class LocalityCollectedRepository : ILocalityCollectedRepository
{
    private readonly DataContext _context;    

    public LocalityCollectedRepository(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));             
    }

    public async Task<IEnumerable<Vehicle>> FindAvailableVehiclesAsync(Guid pickupLocationId, Guid dropOffLocationId, DateTime startDate, DateTime endDate)
    {
        return await _context.Vehicles
            .Join(_context.Availabilities, vehicle => vehicle.Id, availability => availability.VehicleId, (vehicle, availability) => new { vehicle, availability })
            .Where(va => va.availability.PickupLocationId == pickupLocationId &&
                 va.availability.DropOffLocationId == dropOffLocationId &&
                 va.availability.StartDate <= endDate && startDate <= va.availability.EndDate)

            .Select(va => va.vehicle)
            .Distinct()
            .ToListAsync();
    }


    public async Task<IEnumerable<Location>> FindLocationsByCityOrPlaceNameAsync(string searchQuery)
    {
        return await _context.Locations
                .Where(l => EF.Functions.Like(l.City, $"%{searchQuery}%") || EF.Functions.Like(l.PlaceName, $"%{searchQuery}%"))
                .ToListAsync();
    }

}
