using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using MediatR;
using MentorAI.Shared;
using MentorAI.Shared.Enumerations;
using Subscription.Application.Features.SubscriptionPlan.DeletePlan;
using Subscription.Application.Features.SubscriptionPlan.EditPlan;

namespace Subscription.API.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/subscription/plan")]
[Authorize]
public class SubscriptionPlanController : ControllerBase
{
	private readonly Presenter _presenter;
	private readonly IMediator _mediator;

	public SubscriptionPlanController(Presenter presenter, IMediator mediator)
	{
		_presenter = presenter;
		_mediator = mediator;
	}
	
	[HttpPost, Route("create")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	[Authorize(nameof(RoleType.Admin))]
	public async Task<IActionResult> CreatePlanAsync([FromBody, Required] CreatePlanCommand command)
	{
		var result = await _mediator.Send(command);
		
		return _presenter.Handle(result);
	}
	
	[HttpPost, Route("edit")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	[Authorize(nameof(RoleType.Admin))]
	public async Task<IActionResult> EditPlanAsync([FromBody, Required] EditPlanCommand command)
	{
		var result = await _mediator.Send(command);
		
		return _presenter.Handle(result);
	}
	
	[HttpPost, Route("delete")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	[Authorize(nameof(RoleType.Admin))]
	public async Task<IActionResult> DeletePlanAsync([FromBody, Required] DeletePlanCommand command)
	{
		var result = await _mediator.Send(command);
		
		return _presenter.Handle(result);
	}
}