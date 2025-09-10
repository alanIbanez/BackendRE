using Application.DTOs;
using AutoMapper;
using Core.Entities;

namespace Application.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Event, EventDto>().ReverseMap();
    }
}