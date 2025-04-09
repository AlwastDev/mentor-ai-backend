using System.ComponentModel.DataAnnotations;
using MediatR;
using MentorAI.Shared.Attributes;
using MentorAI.Shared.Responses;

namespace Subscription.Application.Features.StudentSubscriptionPlan.AddStudentSubscription;

public record AddStudentSubscriptionCommand([RequiredGuid] Guid PlanId, [Required] string StudentId)
    : IRequest<ResultResponse>;