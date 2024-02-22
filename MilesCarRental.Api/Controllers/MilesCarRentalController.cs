using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilesCarRental.Api.ApiHandlers;
using MilesCarRental.Application.Features.Vehicles.Queries.GetVehicles;
using MilesCarRental.Domain.Ports;

namespace MilesCarRental.Api.Controllers
{
    /// <summary>
    /// Se define el controlador para la funcionalidad del alquiler de vehículos.    
    /// </summary>    
    [Route("api/[controller]")]
    [ApiController]
    public class MilesCarRentalController : ControllerBase
    {
        // Se inyecta las dependencias necesarias a través del constructor.
        private readonly IMediator _mediator;
        private readonly ILogMessageService _resourceManager;
        
        public MilesCarRentalController(IMediator mediator, ILogMessageService resourceManager)
        {
            _mediator = mediator;
            _resourceManager = resourceManager;
        }

        // Endpoint para obtener vehículos. Utiliza el patrón CQRS a través de MediatR.
        [HttpGet("vehicles")]
        public async Task<IActionResult> GetVehicles([FromQuery] GetVehiclesQuery query)
        {
            GetVehiclesQueryValidator validator = new GetVehiclesQueryValidator();
            ValidationResult validationResult = validator.Validate(query);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errorMessages);
            }
            
            var result = await _mediator.Send(query);

            // Retorna una respuesta adecuada basada en si se encontraron vehículos o no.
            return result.Any() ? Ok(result) : Ok(_resourceManager.VehiclesNotFoundMessage);
        }
    }
}
