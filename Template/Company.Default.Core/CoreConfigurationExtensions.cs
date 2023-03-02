using $safeprojectname$.AutoMapper.Profiles;
using $safeprojectname$.Services;
using $safeprojectname$.Validations;
using $ext_safeprojectname$.Domain.Contracts.Services;
using $ext_safeprojectname$.Domain.Entities;
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
                cfg.AddProfile<DtoToEntityProfile>();
            });

            //Validations
            services.AddScoped<IValidator<Person>, PersonValidator>();

            //Services
            services.AddScoped<ICrudService<Person, long>, PersonCrudService>();
            services.AddScoped<IPersonService, PersonService>();
        }
    }
}
