using AutoMapper;
using Course.Application.Dto;
using Course.Domain.Entities;

namespace Course.Application.Mapping;

public class TestMapper : Profile
{
    public TestMapper()
    {
        CreateMap<Test, TestDto>();
    }
}