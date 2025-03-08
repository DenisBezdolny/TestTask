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

            return services;
        }

    }
}
