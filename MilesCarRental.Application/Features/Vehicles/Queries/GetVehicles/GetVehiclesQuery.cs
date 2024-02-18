using MediatR;
using MilesCarRental.Domain.Dtos;

namespace MilesCarRental.Application.Features.Vehicles.Queries.GetVehicles;

public record GetVehiclesQuery(
    Guid pickupLocationId,
    Guid dropOffLocationId,
    DateTime startDate,
    DateTime endDate,
    Guid marketId,
    string locationSearchQuery = ""
) : IRequest<IEnumerable<VehicleDto>>;

