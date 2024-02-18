﻿using FluentValidation;
using MilesCarRental.Application.Features.Vehicles.Queries.GetVehicles;

namespace MilesCarRental.Api.ApiHandlers;
public class GetVehiclesQueryValidator : AbstractValidator<GetVehiclesQuery>
{
    public GetVehiclesQueryValidator()
    {        
        RuleFor(query => query.pickupLocationId).NotEmpty().WithMessage("Pickup location is required.");
        RuleFor(query => query.dropOffLocationId).NotEmpty().WithMessage("Drop-off location is required.");
        RuleFor(query => query.startDate).LessThan(query => query.endDate).WithMessage("Start date must be before end date.");
        RuleFor(query => query.endDate).GreaterThan(query => query.startDate).WithMessage("End date must be after start date.");
        RuleFor(query => query.marketId).NotEmpty().WithMessage("Market ID is required.");     
    }
}
