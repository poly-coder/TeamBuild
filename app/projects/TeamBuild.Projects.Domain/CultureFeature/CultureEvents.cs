using TeamBuild.Core.Domain;

namespace TeamBuild.Projects.Domain.CultureFeature;

public abstract record CultureDomainEvent : IDomainEvent;

public record CultureCreatedEvent(CultureEntity Culture) : CultureDomainEvent;

public record CultureUpdatedEvent(CultureEntity Culture) : CultureDomainEvent;

public record CultureDeletedEvent : CultureDomainEvent;
