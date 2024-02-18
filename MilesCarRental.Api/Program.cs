using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilesCarRental.Api.ApiHandlers;
using MilesCarRental.Api.Middleware;
using MilesCarRental.Infrastructure.DataSource;
using MilesCarRental.Infrastructure.Extensions;
using System.Reflection;
using System.Resources;


public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
        var config = builder.Configuration;

        builder.Services.AddValidatorsFromAssemblyContaining<GetVehiclesQueryValidator>(ServiceLifetime.Transient);

        builder.Services.AddHttpClient();
        builder.Services.AddControllers();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });


        builder.Services.LoadServices(builder.Configuration);
        
        builder.Services.AddDbContext<DataContext>(opts =>
        {
            opts.UseSqlServer(config.GetConnectionString("db"));
        });

        // Registra Fluent Validation
        builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<GetVehiclesQueryValidator>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<ResourceManager>(new ResourceManager("MilesCarRental.Infrastructure.Resources.ErrorMessages", typeof(Program).Assembly));
        builder.Services.AddSingleton<ResourceManager>(new ResourceManager("MilesCarRental.Infrastructure.Resources.SettingMessages", typeof(Program).Assembly));
        builder.Services.AddSingleton<ResourceManager>(new ResourceManager("MilesCarRental.Infrastructure.Resources.Criteria", typeof(Program).Assembly));


        // Registra MediatR        
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("MilesCarRental.Application")));


        // Otras configuraciones...
        var app = builder.Build();

        // Configuración del pipeline de la aplicación...
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseMiddleware<AppExceptionHandlerMiddleware>();
        app.UseHttpsRedirection();
        app.MapControllers();
        app.Run();
    }
}
