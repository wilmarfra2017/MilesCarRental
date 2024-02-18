using MilesCarRental.Domain.Ports;
using MilesCarRental.Infrastructure.Resources;

namespace MilesCarRental.Infrastructure.Adapters
{
    internal class LogMessageService : ILogMessageService
    {
        public string VehiclesNotFoundMessage => SettingMessages.VEHICLE_NOT_MATCH_CRITERIA;

        public string MarketCriteriaIdNotValid => ErrorMessages.VALIDATION_CRITERIA;

        public string VehicleCannotBeNull => ErrorMessages.VALIDATION_VEHICLE;

        public string TypeOneVehicle => Criteria.TYPE_ONE_VEHICLE;

        public string TypeTwoVehicle => Criteria.TYPE_TWO_VEHICLE;

        public string PriceUnder => Criteria.PRICE_PER_DAY;
    }
}
