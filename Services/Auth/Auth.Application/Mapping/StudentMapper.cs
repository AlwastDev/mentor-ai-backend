using Auth.Application.Dto;
using Auth.Domain.Entities;
using AutoMapper;

namespace Auth.Application.Mapping;

public class StudentMapper : Profile
{
    public StudentMapper()
    {
        CreateMap<Student, StudentDto>();
    }
}