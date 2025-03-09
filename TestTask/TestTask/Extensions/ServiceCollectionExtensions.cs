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
            // Registers the generic repository interface IRepository<T> to its concrete implementation Repository<T>.
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


            // Registers the employee factory. 
            services.AddScoped<IEmployeeFactory, EmployeeFactory>();

            // Registers the employee import service, which handles CSV parsing and importing Employee data.
            services.AddScoped<IEmployeeImportService, EmployeeImportService>();

            return services;
        }

    }
}
