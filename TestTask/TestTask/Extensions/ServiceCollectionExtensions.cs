using TestTask.BLL.Factories;
using TestTask.BLL.Services;

using TestTask.Domain.Interfaces.BLL;
using TestTask.Domain.Interfaces.Fabrics;
using TestTask.Domain.Interfaces.Repositories;
using TestTask.Infrastructure.Repositories.Repositories;

namespace TestTask.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// Registers all application services and repositories. Returns the configured service collection
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IEmployeeFactory, EmployeeFactory>();
            services.AddScoped<IEmployeeImportService, EmployeeImportService>();

            return services;
        }

    }
}
