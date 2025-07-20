using JasperFx.Events;
using TeamBuild.Core.Domain;

namespace TeamBuild.Projects.Domain.CultureFeature;

public abstract record CultureEventDao;

public record CultureCreatedEventDao(string EnglishName, string NativeName) : CultureEventDao;

public record CultureUpdatedEventDao(string EnglishName, string NativeName) : CultureEventDao;

public record CultureDeletedEventDao : CultureEventDao;

internal static class CultureDomainEventDaoMapper
{
    public static CultureDomainEvent MapToDomain(this IEvent<CultureEventDao> @event)
    {
        return @event switch
        {
            IEvent<CultureCreatedEventDao> created => created.MapToDomain(),
            IEvent<CultureUpdatedEventDao> updated => updated.MapToDomain(),
            IEvent<CultureDeletedEventDao> deleted => deleted.MapToDomain(),
            _ => throw DomainException.UnexpectedCaseType("event", @event),
        };
    }

    public static CultureCreatedEvent MapToDomain(this IEvent<CultureCreatedEventDao> @event)
    {
        return new CultureCreatedEvent(
            @event.StreamKey!,
            @event.Data.EnglishName,
            @event.Data.NativeName
        );
    }

    public static CultureUpdatedEvent MapToDomain(this IEvent<CultureUpdatedEventDao> @event)
    {
        return new CultureUpdatedEvent(
            @event.StreamKey!,
            @event.Data.EnglishName,
            @event.Data.NativeName
        );
    }

    public static CultureDeletedEvent MapToDomain(this IEvent<CultureDeletedEventDao> @event)
    {
        return new CultureDeletedEvent(@event.StreamKey!);
    }

    public static CultureEventDao MapToDao(this CultureDomainEvent @event)
    {
        return @event switch
        {
            CultureCreatedEvent created => created.MapToDao(),
            CultureUpdatedEvent updated => updated.MapToDao(),
            CultureDeletedEvent deleted => deleted.MapToDao(),
            _ => throw DomainException.UnexpectedCaseType("event", @event),
        };
    }

    public static CultureCreatedEventDao MapToDao(this CultureCreatedEvent @event)
    {
        return new CultureCreatedEventDao(@event.EnglishName, @event.NativeName);
    }

    public static CultureUpdatedEventDao MapToDao(this CultureUpdatedEvent @event)
    {
        return new CultureUpdatedEventDao(@event.EnglishName, @event.NativeName);
    }

    public static CultureDeletedEventDao MapToDao(this CultureDeletedEvent @event)
    {
        return new CultureDeletedEventDao();
    }
}
