using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace TeamBuild.Core;

public static partial class RegexExtensions
{
    public static string ExtractGroup(this string input, Regex pattern, string groupName)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(pattern);
        ArgumentNullException.ThrowIfNull(groupName);

        if (TryExtractGroup(input, pattern, groupName, out var result))
            return result;

        throw new InvalidOperationException(
            $"No match found for pattern '{pattern}' in input '{input}'."
        );
    }

    public static bool TryExtractGroup(
        this string input,
        Regex pattern,
        string groupName,
        [NotNullWhen(true)] out string? result
    )
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(pattern);
        ArgumentNullException.ThrowIfNull(groupName);

        var match = pattern.Match(input);

        if (!match.Success)
        {
            result = null;
            return false;
        }

        result = match.Groups[groupName].Value;
        return true;
    }
}
