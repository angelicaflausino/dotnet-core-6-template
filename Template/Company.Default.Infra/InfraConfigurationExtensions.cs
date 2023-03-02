using $ext_safeprojectname$.Domain.Contracts.Repositories;
using $safeprojectname$.Base;
using $safeprojectname$.Contexts;
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
