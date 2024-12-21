using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickNoteVault.DAL;

namespace QuickNoteVault.BLL;

public static class BusinessLogicLayerExtensions
{
    public static void AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddContext(configuration);

        services.AddUnitOfWork();
    }

    public static void AddServices(this IServiceCollection services)
    {
    }

    public static void InitializeDatabaseIfNotExists(this IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        DbInitializer.Initialize(context).Wait();
    }

    public static void MigrateDatabase(this IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
}
