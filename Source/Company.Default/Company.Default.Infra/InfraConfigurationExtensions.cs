using Company.Default.Domain.Contracts.Repositories;
using Company.Default.Infra.Base;
using Company.Default.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfraConfigurationExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                //options.UseSqlServer()
                options.UseInMemoryDatabase("DbDefault");
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
