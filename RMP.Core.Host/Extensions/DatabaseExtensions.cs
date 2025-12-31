using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Database;

namespace RMP.Core.Host.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddSQLDatabaseConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("SQLConnection")));

        return services;
    }
}