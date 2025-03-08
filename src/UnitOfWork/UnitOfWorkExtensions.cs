using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UnitOfWork;

public static class UnitOfWorkExtensions
{
    public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<DbContext, TContext>();
        return services;
    }
}
