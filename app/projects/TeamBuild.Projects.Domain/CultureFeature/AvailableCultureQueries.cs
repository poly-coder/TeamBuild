using TeamBuild.Core.Domain;

namespace TeamBuild.Projects.Domain.CultureFeature;

public abstract record AvailableCultureQuery : IDomainQuery;

public abstract record AvailableCultureQuerySuccess : IDomainQuerySuccess;

public record AvailableCultureListQuery(string? Search) : AvailableCultureQuery;

public record AvailableCultureListQuerySuccess(IReadOnlyList<CultureDetails> Cultures)
    : AvailableCultureQuerySuccess;
