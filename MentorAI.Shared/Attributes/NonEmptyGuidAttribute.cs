using System.ComponentModel.DataAnnotations;

namespace MentorAI.Shared.Attributes;

public class NonEmptyGuidAttribute : ValidationAttribute
{
	protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
	{
		if (value is null or "")
		{
			return ValidationResult.Success!;
		}

		if (value is Guid guid && Guid.Empty == guid)
		{
			return new("Guid cannot be empty.");
		}

		return ValidationResult.Success!;
	}
}