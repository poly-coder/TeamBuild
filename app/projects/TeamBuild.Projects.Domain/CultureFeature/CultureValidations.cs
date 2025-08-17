using System.Text.RegularExpressions;
using TeamBuild.Core.Domain.FluentValidations;

namespace TeamBuild.Projects.Domain.CultureFeature;

public static partial class CultureValidations
{
    [GeneratedRegex(@"^([a-z][a-z0-9]+)(\-([a-z][a-z0-9]+))*$", RegexOptions.IgnoreCase)]
    internal static partial Regex CultureCodeRegex();

    public static readonly StringValidator CultureCode = StringValidations.Create(
        new(
            MaxLength: 10,
            Pattern: CultureCodeRegex(),
            PatternMessage: value => "Invalid culture code."
        )
    );

    public static readonly StringValidator EnglishName = StringValidations.DisplayName;

    public static readonly StringValidator NativeName = StringValidations.DisplayName;
}
