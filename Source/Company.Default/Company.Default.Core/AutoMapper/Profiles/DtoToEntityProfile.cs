using AutoMapper;
using Company.Default.Domain.Dtos;
using Company.Default.Domain.Entities;

namespace Company.Default.Core.AutoMapper.Profiles
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
