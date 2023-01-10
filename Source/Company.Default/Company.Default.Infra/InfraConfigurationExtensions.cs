using Company.Default.Infra.Base;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfraConfigurationExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
