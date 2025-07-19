namespace TeamBuild.Core.Domain;

public abstract class DomainError
{
    public static DomainError Conflict(string entityType, string entityId) { }

    public static DomainError NotFound(string entityType, string entityId) { }
}
