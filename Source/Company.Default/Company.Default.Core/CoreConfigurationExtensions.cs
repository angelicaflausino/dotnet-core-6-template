using Company.Default.Core.AutoMapper.Profiles;
using Company.Default.Core.Services;
using Company.Default.Core.Validations;
using Company.Default.Domain.Entities;
using Company.Default.Domain.Services;
using FluentValidation;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CoreConfigurationExtensions
    {
        public static void AddCore(this IServiceCollection services)
        {
            //AutoMapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<EntityToDtoProfile>();
            });

            //Validations
            services.AddScoped<IValidator<Person>, PersonValidator>();

            //Services
            services.AddScoped<ICrudService<Person>, PersonCrudService>();
            services.AddScoped<IPersonService, PersonService>();
        }
    }
}
