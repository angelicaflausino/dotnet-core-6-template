using Company.Default.Core.Services;
using Company.Default.Domain.Entities;
using Company.Default.Domain.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CoreConfigurationExtensions
    {
        public static void AddCore(this IServiceCollection services)
        {
            //Crud Services
            services.AddScoped<ICrudService<Person>, PersonCrudService>();
        }
    }
}
