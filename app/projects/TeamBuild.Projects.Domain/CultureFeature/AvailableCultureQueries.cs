using TeamBuild.Core.Domain;

namespace TeamBuild.Projects.Domain.CultureFeature;

public abstract record AvailableCultureQuery : IDomainQuery;

public abstract record AvailableCultureQuerySuccess : IDomainQuerySuccess;

public record AvailableCultureListQuery(AvailableCultureListQueryFilter? Filter = null)
    : AvailableCultureQuery;

public record AvailableCultureListQueryFilter(string? Search = null) : AvailableCultureQuery;

public record AvailableCultureListQuerySuccess(IReadOnlyList<CultureDetails> CultureList)
    : AvailableCultureQuerySuccess;
