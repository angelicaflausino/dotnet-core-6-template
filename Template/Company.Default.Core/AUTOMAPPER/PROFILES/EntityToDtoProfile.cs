using AutoMapper;
using $ext_safeprojectname$.Domain.Dtos;
using $ext_safeprojectname$.Domain.Entities;

namespace $safeprojectname$.AutoMapper.Profiles
{
    public class EntityToDtoProfile : Profile
    {
        public EntityToDtoProfile()
        {
            CreateMap<Person, PersonDto>()
                .ForMember(dto => dto.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        }
    }
}
