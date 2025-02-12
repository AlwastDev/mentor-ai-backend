namespace MentorAI.Shared.Enumerations;

public enum ResultCode : byte
{
    Ok,
    Accepted,
    Created,
    Conflict,
    NotFound,
    UnAuthorize,
    Forbidden,
    LockedOut,
    BadRequest,
    NotModified,
    InternalServerError
}