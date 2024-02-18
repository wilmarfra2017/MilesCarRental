using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilesCarRental.Api.ApiHandlers;
using MilesCarRental.Application.Features.Vehicles.Queries.GetVehicles;
using MilesCarRental.Domain.Ports;

namespace MilesCarRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MilesCarRentalController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogMessageService _resourceManager;

        public MilesCarRentalController(IMediator mediator, ILogMessageService resourceManager)
        {
            _mediator = mediator;
            _resourceManager = resourceManager;
        }

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
            return result.Any() ? Ok(result) : Ok(_resourceManager.VehiclesNotFoundMessage);
        }
    }
}
