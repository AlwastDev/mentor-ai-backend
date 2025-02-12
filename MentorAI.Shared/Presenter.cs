using MentorAI.Shared.Responses;
using MentorAI.Shared.Serialization;

namespace MentorAI.Shared;

public class Presenter
{
    private JsonContentResult ContentResult { get; }

    public Presenter()
    {
        ContentResult = new();
    }

    public JsonContentResult Handle(ResultResponse response)
    {
        ContentResult.StatusCode = (int)ResultConverter.GetHttpStatusCode(response.ResultCode);
        ContentResult.Content = JsonSerializer.SerializeObject(response.Message);

        return ContentResult;
    }

    public JsonContentResult Handle<T>(ResultResponse<T> response)
    {
        ContentResult.StatusCode = (int)ResultConverter.GetHttpStatusCode(response.ResultCode);

        if (response.Data is not null)
        {
            ContentResult.Content = JsonSerializer.SerializeObject(response.Data);
        }
        else
        {
            ContentResult.Content = JsonSerializer.SerializeObject(response.Message);
        }

        return ContentResult;
    }

    public JsonContentResult Handle<T>(ResultArrayResponse<T> response)
    {
        ContentResult.StatusCode = (int)ResultConverter.GetHttpStatusCode(response.ResultCode);

        if (response.Data is not null)
        {
            if (!string.IsNullOrEmpty(response.Data.Name))
            {
                ContentResult.Content = JsonSerializer.SerializeObject(response.Data, response.Data.Name);
            }
            else
            {
                ContentResult.Content = JsonSerializer.SerializeObject(response.Data);
            }
        }
        else
        {
            ContentResult.Content = JsonSerializer.SerializeObject(response.Message);
        }

        return ContentResult;
    }
}