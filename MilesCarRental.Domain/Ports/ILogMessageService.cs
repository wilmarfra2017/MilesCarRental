namespace MilesCarRental.Domain.Ports
{
    public interface ILogMessageService
    {
        string VehiclesNotFoundMessage { get; }
        string MarketCriteriaIdNotValid { get; }
        string VehicleCannotBeNull { get; }
        string TypeOneVehicle { get; }
        string TypeTwoVehicle { get; }
        string PriceUnder { get; }
    }
}
