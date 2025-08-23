namespace TeamBuild.Core.Application;

public static class TeamBuildCoreApplicationQueryExtensions
{
    public static IEnumerable<TItem> ApplyTextSearchData<TItem>(
        this IEnumerable<TItem> query,
        string? search,
        Func<TItem, string> getTextSearchData
    )
    {
        if (search is not null)
        {
            var textSearchQuery = search.ToTextSearchQuery();

            return query.Where(e =>
            {
                var textSearchData = getTextSearchData(e);
                return textSearchQuery.Any(q => textSearchData.Contains(q));
            });
        }

        return query;
    }
}
