using Application.Common.UnitOfWork;
using Infrastructure.Persistence;
using Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CoursesDbContext>((provider, builder) =>
        {
            builder.UseNpgsql(configuration.GetConnectionString("PostgreSQL"));
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}