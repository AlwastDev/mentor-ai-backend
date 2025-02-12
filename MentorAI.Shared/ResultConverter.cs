using System.Net;
using MentorAI.Shared.Enumerations;

namespace MentorAI.Shared;

public static class ResultConverter
{
    public static HttpStatusCode GetHttpStatusCode(ResultCode code) =>
        code switch
        {
            ResultCode.Ok => HttpStatusCode.OK,
            ResultCode.Accepted => HttpStatusCode.Accepted,
            ResultCode.Created => HttpStatusCode.Created,
            ResultCode.Conflict => HttpStatusCode.Conflict,
            ResultCode.NotFound => HttpStatusCode.NotFound,
            ResultCode.BadRequest => HttpStatusCode.BadRequest,
            ResultCode.Forbidden => HttpStatusCode.Forbidden,
            ResultCode.LockedOut => HttpStatusCode.Locked,
            ResultCode.UnAuthorize => HttpStatusCode.Unauthorized,
            ResultCode.NotModified => HttpStatusCode.NotModified,
            _ => HttpStatusCode.InternalServerError
        };
}