using MentorAI.Shared.Enumerations;

namespace MentorAI.Shared.Responses;

public record ResultResponse
{
    public ResultCode ResultCode { get; }
    public string? Message { get; }

    public ResultResponse(ResultCode resultCode)
    {
        ResultCode = resultCode;
    }

    public ResultResponse(ResultCode resultCode, string message)
    {
        ResultCode = resultCode;
        Message = message;
    }
}

public record ResultResponse<T> : ResultResponse
{
    public T? Data { get; }

    public ResultResponse(ResultCode resultCode, T? data = default) : base(resultCode)
    {
        Data = data;
    }

    public ResultResponse(ResultCode resultCode, string message, T? data = default) : base(resultCode, message)
    {
        Data = data;
    }
}

public record ResultArrayResponse<T> : ResultResponse<ResponseArray<T>>
{
    public ResultArrayResponse(ResultCode resultCode, IEnumerable<T>? data = null, string? name = null)
        : base(resultCode, new() { Items = data, Name = name }) { }

    public ResultArrayResponse(ResultCode resultCode, string message, IEnumerable<T>? data = null, string? name = null)
        : base(resultCode, message, new() { Items = data, Name = name }) { }
}

public record PaginatedResponse<T> : ResultResponse<PaginatedArray<T>>
{
    public PaginatedResponse(ResultCode resultCode, string message) : base(resultCode, message) { }

    public PaginatedResponse(ResultCode resultCode, IEnumerable<T> data, int totalCount) : base(resultCode, new() { Items = data, TotalCount = totalCount }) { }
}