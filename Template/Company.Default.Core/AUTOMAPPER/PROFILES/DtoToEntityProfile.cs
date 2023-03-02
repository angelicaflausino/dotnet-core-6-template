using AutoMapper;
using $ext_safeprojectname$.Domain.Dtos;
using $ext_safeprojectname$.Domain.Entities;

namespace $safeprojectname$.AutoMapper.Profiles
{
    public class DtoToEntityProfile : Profile
    {
        public DtoToEntityProfile()
        {
            CreateMap<PersonDto, Person>()
                .ForMember(dest => dest.Enabled, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
