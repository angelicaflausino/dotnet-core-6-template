using Company.Default.Core.AutoMapper.Profiles;
using Company.Default.Core.Services;
using Company.Default.Core.Validations;
using Company.Default.Domain.Contracts.Services;
using Company.Default.Domain.Entities;
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
