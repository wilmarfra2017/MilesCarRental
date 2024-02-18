namespace MilesCarRental.Domain.Dtos;

public record VehicleDto(
    Guid Id,
    string Brand,
    string Model,
    int Year,
    string Type
);
