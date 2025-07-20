using TeamBuild.Core.Domain;

namespace TeamBuild.Projects.Domain.CultureFeature;

public abstract record CultureDomainEvent : IDomainEvent;

public record CultureCreatedEvent(string CultureCode, string EnglishName, string NativeName)
    : CultureDomainEvent;

public record CultureUpdatedEvent(string CultureCode, string EnglishName, string NativeName)
    : CultureDomainEvent;

public record CultureDeletedEvent(string CultureCode) : CultureDomainEvent;
