using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MilesCarRental.Domain.Dtos;
using MilesCarRental.Domain.Entities;
using MilesCarRental.Infrastructure.DataSource;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;

namespace MilesCarRental.Api.Tests
{
    /// <summary>
    /// Clase de pruebas para el controlador de MilesCarRental, se enfoca en probar los endpoints de la API
    /// asegurando que respondan correctamente bajo diversas condiciones.
    /// </summary>
    public class MilesCarRentalControllerTests : IClassFixture<MilesCarRentalApiApp>
    {
        private readonly MilesCarRentalApiApp _factory;

        public MilesCarRentalControllerTests(MilesCarRentalApiApp factory)
        {
            _factory = factory;
        }

        // Prueba para verificar que la respuesta del endpoint GetVehicles es 200 OK con los parámetros adecuados.
        [Fact]
        public async Task GetVehicles_ReturnsVehiclesResponse200()
        {
            // Arrange
            var client = _factory.CreateClient();
            var requestUri = "/api/MilesCarRental/vehicles";
            var pickupLocationId = Guid.NewGuid();
            var dropOffLocationId = Guid.NewGuid();
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddDays(1);
            var marketId = Guid.NewGuid();

            requestUri += $"?pickupLocationId={pickupLocationId}&dropOffLocationId={dropOffLocationId}&startDate={startDate:O}&endDate={endDate:O}&marketId={marketId}";

            // Act
            var response = await client.GetAsync(requestUri);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // Prueba para verificar que la respuesta del endpoint GetVehicles devuelve una lista de vehículos.
        [Fact]
        public async Task GetVehicles_ReturnsVehiclesResponse()
        {
            await PrepareDatabaseAsync();

            // Arrange
            var client = _factory.CreateClient();

            var pickupLocationId = Guid.Parse("F84BF18B-7BE7-4F0D-B325-62987D94B229").ToString().ToUpper();
            var dropOffLocationId = Guid.Parse("F84BF18B-7BE7-4F0D-B325-62987D94B229").ToString().ToUpper();

            var startDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var endDate = new DateTime(2023, 1, 10, 0, 0, 0, DateTimeKind.Utc);

            var marketId = Guid.Parse("3C021872-17F6-4267-A5D0-BC3F63D7CEC8");

            // Construye la URI con los parámetros adecuados
            var requestUri = $"/api/MilesCarRental/vehicles?pickupLocationId={pickupLocationId}&dropOffLocationId={dropOffLocationId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&marketId={marketId}";

            // Act
            var response = await client.GetAsync(requestUri);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            

            var vehicles = await response.Content.ReadFromJsonAsync<IEnumerable<VehicleDto>>();
            Assert.NotNull(vehicles);
            Assert.NotEmpty(vehicles);
        }

        // Prepara la base de datos in-memory para las pruebas, asegurando que los datos necesarios estén presentes.
        private async Task PrepareDatabaseAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            
            var vehicle = await CreateAndAddVehicleAsync(dbContext, "Renault", "Duster", "SUV", 2020, 40);

            await CreateAndAddAvailabilityAsync(dbContext, vehicle.Id, "F84BF18B-7BE7-4F0D-B325-62987D94B229", "2023-01-01", "2023-01-10");

            await CreateAndAddMarketCriteriaAsync(dbContext, "3C021872-17F6-4267-A5D0-BC3F63D7CEC8", "SUV,Electric,PriceUnder50");

            await VerifyInsertionAsync(dbContext, vehicle.Id, "3C021872-17F6-4267-A5D0-BC3F63D7CEC8");
        }

        // Crea y añade un vehículo a la base de datos in-memory.
        private async Task<Vehicle> CreateAndAddVehicleAsync(DataContext dbContext, string brand, string model, string type, int year, decimal pricePerDay)
        {
            var vehicle = new Vehicle { Brand = brand, Model = model, Type = type, Year = year, PricePerDay = pricePerDay };
            dbContext.Vehicles.Add(vehicle);
            await dbContext.SaveChangesAsync();
            return vehicle;
        }

        // Crea y añade información de disponibilidad para un vehículo en la base de datos in-memory.
        private async Task CreateAndAddAvailabilityAsync(DataContext dbContext, Guid vehicleId, string locationId, string startDateStr, string endDateStr)
        {
            var availability = new Availability
            {
                VehicleId = vehicleId,
                PickupLocationId = Guid.Parse(locationId),
                DropOffLocationId = Guid.Parse(locationId),
                StartDate = DateTime.ParseExact(startDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                EndDate = DateTime.ParseExact(endDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture)
            };
            dbContext.Availabilities.Add(availability);
            await dbContext.SaveChangesAsync();
        }

        // Crea y añade criterios de mercado en la base de datos in-memory.
        private async Task CreateAndAddMarketCriteriaAsync(DataContext dbContext, string criteriaId, string criteria)
        {
            var marketCriteria = new MarketCriteria { Id = Guid.Parse(criteriaId), Criteria = criteria };
            dbContext.MarketCriterias.Add(marketCriteria);
            await dbContext.SaveChangesAsync();
        }

        // Verifica la inserción de vehículos y sus criterios de mercado en la base de datos in-memory.
        private async Task VerifyInsertionAsync(DataContext dbContext, Guid vehicleId, string criteriaId)
        {
            var insertedVehicle = await dbContext.Vehicles.FindAsync(vehicleId);
            Assert.NotNull(insertedVehicle);

            var insertedAvailability = await dbContext.Availabilities.FirstOrDefaultAsync(a => a.VehicleId == vehicleId);
            Assert.NotNull(insertedAvailability);

            var insertedCriteria = await dbContext.MarketCriterias.FindAsync(Guid.Parse(criteriaId));
            Assert.NotNull(insertedCriteria);
        }
    }
}
