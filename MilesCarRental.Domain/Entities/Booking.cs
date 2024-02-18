namespace MilesCarRental.Domain.Entities;

public class Booking : DomainEntity
{
    public Guid CustomerId { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime PickupDateTime { get; set; }
    public DateTime DropOffDateTime { get; set; }
    public Guid PickupLocationId { get; set; }
    public Guid DropOffLocationId { get; set; }
}
