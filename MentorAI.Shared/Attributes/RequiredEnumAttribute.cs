using System.ComponentModel.DataAnnotations;

namespace MentorAI.Shared.Attributes;

public class RequiredEnumAttribute : RequiredAttribute
{
	public override bool IsValid(object? value)
	{
		if (value is null)
		{
			return false;
		}

		var type = value.GetType();

		return type.IsEnum && Enum.IsDefined(type, value);
	}
}