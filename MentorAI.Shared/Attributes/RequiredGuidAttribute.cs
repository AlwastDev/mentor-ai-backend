using System.ComponentModel.DataAnnotations;

namespace MentorAI.Shared.Attributes;

public class RequiredGuidAttribute : RequiredAttribute
{
	public override bool IsValid(object? value) => value is Guid guid && guid != Guid.Empty;
}