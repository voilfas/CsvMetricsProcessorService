using CsvMetricsProcessorService.Application.Interfaces;
using CsvMetricsProcessorService.Infrastructure.Persistence;
using CsvMetricsProcessorService.Infrastructure.Persistence.Repositories;
using CsvMetricsProcessorService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace CsvMetricsProcessorService.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddAppService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(ICsvParser).Assembly));
        
        services.AddDbContext<MetricsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IMetricsRepository, MetricsRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IMetricsQueries, MetricsQueries>();
        services.AddScoped<ICsvParser, CsvParser>();
        
        return services;
    }
}