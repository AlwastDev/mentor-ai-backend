using System.ComponentModel.DataAnnotations;
using Auth.Application.Responses;
using MediatR;
using MentorAI.Shared.Responses;

namespace Auth.Application.Features.Auth.Login;

public record LoginCommand(
    [EmailAddress] string Email,
    [Required, DataType(DataType.Password)]
    string Password) : IRequest<ResultResponse<AccessTokenResponse>>;