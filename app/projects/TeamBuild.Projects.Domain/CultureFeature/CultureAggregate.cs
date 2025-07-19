using TeamBuild.Core;
using TeamBuild.Core.Domain;

namespace TeamBuild.Projects.Domain.CultureFeature;

using CultureAggregateResult = Result<IReadOnlyCollection<CultureDomainEvent>, DomainError>;

public record CultureAggregate(CultureEntity Culture)
{
    public const string EntityName = "Culture";

    public static CultureAggregateResult Execute(
        Option<CultureAggregate> aggregate,
        CultureCommand command
    )
    {
        return aggregate switch
        {
            Option<CultureAggregate>.Some { Value: var existing } => command switch
            {
                CultureCreateCommand cmd => Create(existing, cmd),
                CultureUpdateCommand cmd => Update(existing, cmd),
                CultureDeleteCommand cmd => Delete(existing, cmd),

                _ => throw DomainException.UnknownCommandType(command),
            },

            Option<CultureAggregate>.None => command switch
            {
                CultureCreateCommand cmd => Create(cmd),
                CultureUpdateCommand cmd => Update(cmd),
                CultureDeleteCommand cmd => Delete(cmd),

                _ => throw DomainException.UnknownCommandType(command),
            },

            _ => throw DomainException.UnknownCaseType("option", aggregate),
        };
    }

    private static CultureAggregateResult Create(CultureAggregate agg, CultureCreateCommand cmd) =>
        agg.Culture.Equals(cmd.Culture)
            // Idempotency
            ? Array.Empty<CultureDomainEvent>()
            : DomainError.Conflict(EntityName, agg.Culture.CultureCode);

    private static CultureAggregateResult Create(CultureCreateCommand cmd) =>
        new CultureDomainEvent[] { new CultureUpdatedEvent(cmd.Culture) };

    private static CultureAggregateResult Update(CultureAggregate agg, CultureUpdateCommand cmd) =>
        agg.Culture.Equals(cmd.Culture)
            ? Array.Empty<CultureDomainEvent>()
            : [new CultureUpdatedEvent(cmd.Culture)];

    private static CultureAggregateResult Update(CultureUpdateCommand cmd) =>
        DomainError.NotFound(EntityName, cmd.Culture.CultureCode);

    private static CultureAggregateResult Delete(CultureAggregate agg, CultureDeleteCommand cmd) =>
        new CultureDomainEvent[] { new CultureDeletedEvent() };

    private static CultureAggregateResult Delete(CultureDeleteCommand cmd) =>
        Array.Empty<CultureDomainEvent>();
}
