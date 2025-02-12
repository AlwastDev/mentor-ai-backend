using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace MentorAI.Shared;

public sealed class JsonContentResult : ContentResult
{
    public JsonContentResult()
    {
        ContentType = "application/json";
    }

    public JsonContentResult(HttpStatusCode statusCode, string content)
    {
        this.StatusCode = (int)statusCode;
        this.Content = content;
    }
}