namespace MilesCarRental.Domain.Entities;

public class MarketCriteria
{
    public Guid Id { get; set; }    
    public string Criteria { get; set; } = default!;
}
