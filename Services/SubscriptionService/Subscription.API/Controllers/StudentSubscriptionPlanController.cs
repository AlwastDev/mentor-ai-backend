using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using MediatR;
using MentorAI.Shared;
using MentorAI.Shared.Enumerations;
using Subscription.Application.Features.StudentSubscriptionPlan.AddStudentSubscription;

namespace Subscription.API.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/student/subscription/plan")]
[Authorize]
public class StudentSubscriptionPlanController : ControllerBase
{
	private readonly Presenter _presenter;
	private readonly IMediator _mediator;

	public StudentSubscriptionPlanController(Presenter presenter, IMediator mediator)
	{
		_presenter = presenter;
		_mediator = mediator;
	}
	
	[HttpPost, Route("add")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	[Authorize(nameof(RoleType.Student))]
	public async Task<IActionResult> AddStudentSubscriptionPlanAsync([FromBody, Required] AddStudentSubscriptionCommand command)
	{
		var result = await _mediator.Send(command);
		
		return _presenter.Handle(result);
	}
}