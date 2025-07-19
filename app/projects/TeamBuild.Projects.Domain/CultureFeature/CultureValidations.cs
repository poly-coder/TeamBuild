using System.Text.RegularExpressions;
using TeamBuild.Core.Domain.FluentValidations;

namespace TeamBuild.Projects.Domain.CultureFeature;

public static partial class CultureValidations
{
    [GeneratedRegex(@"^([a-z][a-z0-9]+)(\-([a-z][a-z0-9]+))*$", RegexOptions.IgnoreCase)]
    internal static partial Regex CultureCodeRegex();

    public static StringValidator CultureCode = StringValidations.Create(
        new(
            MaxLength: 10,
            Pattern: CultureCodeRegex(),
            PatternMessage: value => "Invalid culture code."
        )
    );

    public static StringValidator EnglishName = StringValidations.DisplayName;

    public static StringValidator NativeName = StringValidations.DisplayName;
}
