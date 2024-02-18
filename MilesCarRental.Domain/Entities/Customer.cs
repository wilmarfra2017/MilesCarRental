﻿namespace MilesCarRental.Domain.Entities;
public class Customer : DomainEntity
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
}
