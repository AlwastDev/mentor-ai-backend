using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MentorAI.Shared;

public static class ValidationHelper
{
    public static ValidationResultModel FormatOutput(ModelStateDictionary modelState)
    {
        var result = new ValidationResultModel(modelState
            .Where(static modelStateEntry => modelStateEntry.Value?.ValidationState == ModelValidationState.Invalid)
            .Select(static modelStateEntry =>
                $"{modelStateEntry.Key.ToLowerFirstChar()}: {string.Join(" ", modelStateEntry.Value!.Errors.Select(static error => error.ErrorMessage))}"));

        return result;
    }
}