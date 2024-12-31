using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickNoteVault.BLL.Services;
using QuickNoteVault.BLL.Services.Interfaces;
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
        services.AddScoped<INoteService, NoteService>();
    }

    public static async Task InitializeDatabaseIfNotExistsAsync(this IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await DbInitializer.Initialize(context);
    }

    public static async Task MigrateDatabaseAsync(this IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
    }

    public static void AddBLLMaps(this IMapperConfigurationExpression configuration)
    {
        configuration.AddMaps(typeof(BusinessLogicLayerExtensions));
    }
}
