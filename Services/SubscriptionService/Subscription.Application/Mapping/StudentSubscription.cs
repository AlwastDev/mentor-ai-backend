using AutoMapper;
using Subscription.Application.Dto;
using Subscription.Domain.Entities;

namespace Subscription.Application.Mapping;

public class StudentSubscriptionMapper : Profile
{
    public StudentSubscriptionMapper()
    {
        CreateMap<StudentSubscription, StudentSubscriptionDto>();
    }
}