using AutoMapper;
using Subscription.Application.Dto;
using Subscription.Domain.Entities;

namespace Subscription.Application.Mapping;

public class SubscriptionPlanMapper : Profile
{
    public SubscriptionPlanMapper()
    {
        CreateMap<SubscriptionPlan, SubscriptionPlanDto>();
        CreateMap<SubscriptionPlanDto, SubscriptionPlan>();
    }
}