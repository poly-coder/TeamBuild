using TeamBuild.Core;
using TeamBuild.Core.Domain;

namespace TeamBuild.Projects.Domain.CultureFeature;

using CultureAggregateResult = Result<IReadOnlyCollection<CultureDomainEvent>, DomainError>;

public record CultureAggregate(string CultureCode, string EnglishName, string NativeName)
{
    public const string EntityName = "Culture";

    #region [ Commands ]

    public static CultureAggregateResult Create(
        Option<CultureAggregate> aggregate,
        CultureCreateCommand command
    ) =>
        aggregate switch
        {
            Option<CultureAggregate>.Some { Value: var agg } => !agg.HasChanges(command)
                // Idempotency
                ? Array.Empty<CultureDomainEvent>()
                : DomainError.Conflict(EntityName, agg.CultureCode),

            Option<CultureAggregate>.None => new CultureDomainEvent[]
            {
                new CultureUpdatedEvent(
                    command.CultureCode,
                    command.EnglishName,
                    command.NativeName
                ),
            },

            _ => throw DomainException.UnexpectedCaseType("option", aggregate),
        };

    private bool HasChanges(CultureCreateCommand command)
    {
        return !CultureCode.Equals(command.CultureCode, StringComparison.Ordinal)
            || !EnglishName.Equals(command.EnglishName, StringComparison.Ordinal)
            || !NativeName.Equals(command.NativeName, StringComparison.Ordinal);
    }

    public static CultureAggregateResult Update(
        Option<CultureAggregate> aggregate,
        CultureUpdateCommand command
    ) =>
        aggregate switch
        {
            Option<CultureAggregate>.Some { Value: var agg } => !agg.HasChanges(command)
                ? Array.Empty<CultureDomainEvent>()
                :
                [
                    new CultureUpdatedEvent(
                        command.CultureCode,
                        command.EnglishName,
                        command.NativeName
                    ),
                ],

            Option<CultureAggregate>.None => DomainError.NotFound(EntityName, command.CultureCode),

            _ => throw DomainException.UnexpectedCaseType("option", aggregate),
        };

    private bool HasChanges(CultureUpdateCommand command)
    {
        return !CultureCode.Equals(command.CultureCode, StringComparison.Ordinal)
            || !EnglishName.Equals(command.EnglishName, StringComparison.Ordinal)
            || !NativeName.Equals(command.NativeName, StringComparison.Ordinal);
    }

    public static CultureAggregateResult Delete(
        Option<CultureAggregate> aggregate,
        CultureDeleteCommand command
    ) =>
        aggregate switch
        {
            Option<CultureAggregate>.Some { Value: var agg } => new CultureDomainEvent[]
            {
                new CultureDeletedEvent(command.CultureCode),
            },

            Option<CultureAggregate>.None => Array.Empty<CultureDomainEvent>(),

            _ => throw DomainException.UnexpectedCaseType("option", aggregate),
        };

    #endregion [ Execute ]

    #region [ Apply ]

    public static Option<CultureAggregate> Apply(
        Option<CultureAggregate> aggregate,
        CultureDomainEvent @event
    )
    {
        return aggregate switch
        {
            Option<CultureAggregate>.Some { Value: var existing } => @event switch
            {
                CultureCreatedEvent ev => ApplyCreated(existing, ev),
                CultureUpdatedEvent ev => ApplyUpdated(existing, ev),
                CultureDeletedEvent ev => ApplyDeleted(existing, ev),

                _ => throw DomainException.UnexpectedCommandType(@event),
            },

            Option<CultureAggregate>.None => @event switch
            {
                CultureCreatedEvent ev => ApplyCreated(ev),
                CultureUpdatedEvent ev => ApplyUpdated(ev),
                CultureDeletedEvent ev => ApplyDeleted(ev),

                _ => throw DomainException.UnexpectedCommandType(@event),
            },

            _ => throw DomainException.UnexpectedCaseType("option", aggregate),
        };
    }

    private static Option<CultureAggregate> ApplyCreated(
        CultureAggregate agg,
        CultureCreatedEvent ev
    ) => new CultureAggregate(ev.CultureCode, ev.EnglishName, ev.NativeName);

    private static Option<CultureAggregate> ApplyCreated(CultureCreatedEvent ev) =>
        new CultureAggregate(ev.CultureCode, ev.EnglishName, ev.NativeName);

    private static Option<CultureAggregate> ApplyUpdated(
        CultureAggregate agg,
        CultureUpdatedEvent ev
    ) => new CultureAggregate(ev.CultureCode, ev.EnglishName, ev.NativeName);

    private static Option<CultureAggregate> ApplyUpdated(CultureUpdatedEvent ev) =>
        Option.None<CultureAggregate>();

    private static Option<CultureAggregate> ApplyDeleted(
        CultureAggregate agg,
        CultureDeletedEvent ev
    ) => Option.None<CultureAggregate>();

    private static Option<CultureAggregate> ApplyDeleted(CultureDeletedEvent ev) =>
        Option.None<CultureAggregate>();

    #endregion [ Apply ]
}
