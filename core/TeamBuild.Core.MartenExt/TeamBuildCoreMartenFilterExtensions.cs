using System.Linq.Expressions;
using System.Reflection;
using CommunityToolkit.Diagnostics;
using TeamBuild.Core.Reflection;

namespace TeamBuild.Core.MartenExt;

public static class TeamBuildCoreMartenFilterExtensions
{
    private static readonly MethodInfo StringContainsMethod = ReflectionExtensions.ExtractMethod(
        (string s) => s.Contains("")
    );

    public static IQueryable<TDocument> ApplyTextSearchData<TDocument>(
        this IQueryable<TDocument> source,
        string? textSearch,
        Expression<Func<TDocument, string>> getTextSearchData
    )
    {
        Guard.IsNotNull(source);
        Guard.IsNotNull(getTextSearchData);

        if (textSearch.ToTextSearchQuery() is { Count: > 0 } terms)
        {
            Guard.IsEqualTo(getTextSearchData.Parameters.Count, 1);

            // For each term, create an expression of the kind: doc.TextSearchData.Contains(term)
            // Combine these expressions with OR
            // Create a lambda expression that Marten can use to filter the documents

            var docParam = getTextSearchData.Parameters[0];
            var docDataProp = getTextSearchData.Body;

            var predicateBody = terms
                .Select(term =>
                    Expression.Call(
                        docDataProp,
                        StringContainsMethod,
                        Expression.Constant(term, typeof(string))
                    )
                )
                .CombineOrElseExpressions();

            var predicate = Expression.Lambda<Func<TDocument, bool>>(predicateBody, docParam);

            source = source.Where(predicate);
        }

        return source;
    }
}
