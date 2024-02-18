namespace MilesCarRental.Domain.Entities;
public class Vehicle : DomainEntity
{
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string Type { get; set; } = default!;
    public int Year { get; set; }
    public decimal PricePerDay { get; set; }
}