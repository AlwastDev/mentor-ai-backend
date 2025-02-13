using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Auth.Application.Features.Auth.Login;
using Auth.Application.Features.Auth.Logout;
using Auth.Application.Features.Auth.RefreshToken;
using Auth.Application.Features.Auth.Register;
using Auth.Application.Responses;
using MediatR;
using MentorAI.Shared;

namespace Auth.API.Controllers;

[Produces("application/json")]
[ApiController]
[Route("api/auth")]
[Authorize]
public class AuthController : ControllerBase
{
	private readonly Presenter _presenter;
	private readonly IMediator _mediator;

	public AuthController(Presenter presenter, IMediator mediator)
	{
		_presenter = presenter;
		_mediator = mediator;
	}
	
	[HttpPost, Route("register")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	[AllowAnonymous]
	public async Task<IActionResult> RegisterAsync([FromBody, Required] RegisterCommand command)
	{
		var result = await _mediator.Send(command);
		
		return _presenter.Handle(result);
	}
	
	[HttpPost, Route("login")]
	[ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	[AllowAnonymous]
	public async Task<IActionResult> LoginAsync([FromBody, Required] LoginCommand command)
	{
		var result = await _mediator.Send(command);
		
		return _presenter.Handle(result);
	}

	[HttpPost, Route("refresh")]
	[ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status400BadRequest)]
	[AllowAnonymous]
	public async Task<IActionResult> RefreshTokenAsync([FromBody, Required] RefreshTokenCommand command)
	{
		var result = await _mediator.Send(command);
		return _presenter.Handle(result);
	}
	
	[HttpPost, Route("logout")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> LogoutAsync()
	{
		var userId = HttpContext.User.Claims.First(static claim => claim.Type == ClaimTypes.NameIdentifier).Value;
	
		var response = await _mediator.Send(new LogoutCommand(userId));
		
		return _presenter.Handle(response);
	}
}