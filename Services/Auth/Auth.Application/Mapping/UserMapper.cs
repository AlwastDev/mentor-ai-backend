using Auth.Application.Dto;
using Auth.Domain.Entities;
using AutoMapper;
using MentorAI.Shared.Enumerations;

namespace Auth.Application.Mapping;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserDto>()
            .ForMember(userDto => userDto.UserRole,
                memberConfigurationExpression => memberConfigurationExpression.MapFrom(user => Enum.Parse(typeof(RoleType), user.Discriminator)));
    }
}