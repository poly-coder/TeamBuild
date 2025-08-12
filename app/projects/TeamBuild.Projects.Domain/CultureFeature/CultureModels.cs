namespace TeamBuild.Projects.Domain.CultureFeature;

public record CultureDetails(string CultureCode, string EnglishName, string NativeName);

public class CultureEntity
{
    public const string Caption = "Culture";
    public const string PluralCaption = "Cultures";
}
