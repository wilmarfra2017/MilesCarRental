namespace MilesCarRental.Domain.Entities;

public class Availability : DomainEntity
{
    public Guid VehicleId { get; set; }
    public Guid PickupLocationId { get; set; }
    public Guid DropOffLocationId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
