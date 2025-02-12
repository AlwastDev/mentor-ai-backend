namespace Auth.Domain.Common;

public enum TokenType : byte
{
    AccessToken,
    RefreshToken,
    RegistrationToken
}