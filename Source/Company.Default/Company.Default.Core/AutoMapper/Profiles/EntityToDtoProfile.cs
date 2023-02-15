using AutoMapper;
using Company.Default.Domain.Dtos;
using Company.Default.Domain.Entities;

namespace Company.Default.Core.AutoMapper.Profiles
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
