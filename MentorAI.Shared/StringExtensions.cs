using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace MentorAI.Shared;

public static class StringExtensions
{
    public static string ToLowerFirstChar(this string input)
    {
        var newString = input;

        if (!string.IsNullOrEmpty(newString) && char.IsUpper(newString[0]))
        {
            newString = char.ToLower(newString[0]) + newString[1..];
        }

        return newString;
    }

    public static string ToBase64(this string input) => WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(input));

    public static string FromBase64(this string input) => Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input));

    public static string Format(this string input) => input.Replace("-", string.Empty);

    public static string Format(this Guid input) => input.ToString().Replace("-", string.Empty);

    public static string JoinWithoutEmpty(this IEnumerable<string?> values, string separator) =>
        string.Join(separator, values.Where(stringElement => !string.IsNullOrWhiteSpace(stringElement)));
}