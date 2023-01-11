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
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.GetAge()));
        }
    }
}
