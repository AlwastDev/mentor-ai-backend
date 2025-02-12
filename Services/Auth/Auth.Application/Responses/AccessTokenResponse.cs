namespace Auth.Application.Responses;

public record AccessTokenResponse(string AccessToken, string RefreshToken);