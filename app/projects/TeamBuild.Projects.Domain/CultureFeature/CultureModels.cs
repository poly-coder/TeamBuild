namespace TeamBuild.Projects.Domain.CultureFeature;

public record CultureDetails(string CultureCode, string EnglishName, string NativeName);

public static class CultureEntity
{
    public const string Caption = "culture";
}
