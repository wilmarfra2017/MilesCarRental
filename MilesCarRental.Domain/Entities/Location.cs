namespace MilesCarRental.Domain.Entities;
public class Location : DomainEntity
{
    public string Address { get; set; } = default!;
    public string City { get; set; } = default!;
    public string StateProvince { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string PlaceName { get; set; } = default!;
}
