using Auth.GRPC.Protos;
using MediatR;
using MentorAI.Shared.Enumerations;
using MentorAI.Shared.Responses;
using Subscription.Application.Contracts.Persistence;
using Subscription.Application.Dto;

namespace Subscription.Application.Features.StudentSubscriptionPlan.AddStudentSubscription;

public class AddStudentSubscriptionHandler : IRequestHandler<AddStudentSubscriptionCommand, ResultResponse>
{
    private readonly IStudentSubscriptionRepository _studentSubscriptionPlanRepository;
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly AuthGrpcService _authGrpcService;

    public AddStudentSubscriptionHandler(IStudentSubscriptionRepository studentSubscriptionPlanRepository,
        ISubscriptionPlanRepository subscriptionPlanRepository,
        AuthGrpcService authGrpcService)
    {
        _studentSubscriptionPlanRepository = studentSubscriptionPlanRepository;
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _authGrpcService = authGrpcService;
    }

    public async Task<ResultResponse> Handle(AddStudentSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var response = await _authGrpcService.CheckStudent(request.StudentId);

        if (!response)
        {
            return new(ResultCode.NotFound, "Student not found");
        }

        var subscription = await _subscriptionPlanRepository.GetPlanByIdAsync(request.PlanId);

        if (subscription is null)
        {
            return new(ResultCode.NotFound, "Plan not found");
        }

        var studentSubscription =
            await _studentSubscriptionPlanRepository
                .GetActiveSubscriptionByStudentIdAsync(request.StudentId);

        if (studentSubscription is not null)
        {
            return new(ResultCode.Conflict, $"Subscription {studentSubscription.Plan.PlanName} already active");
        }

        var studentSubscriptionDto = new StudentSubscriptionDto
        {
            Id = Guid.Empty,
            PlanId = request.PlanId,
            StudentId = request.StudentId,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(subscription.DurationDays)
        };

        await _studentSubscriptionPlanRepository.AddSubscriptionAsync(studentSubscriptionDto);

        return new(ResultCode.Ok);
    }
}