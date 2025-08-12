namespace TeamBuild.Core.Domain;

public class DomainException : Exception
{
    public DomainException() { }

    public DomainException(string message)
        : base(message) { }

    public DomainException(string message, Exception inner)
        : base(message, inner) { }
}

public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityType, string entityId)
        : base($"Entity '{entityType}' with ID '{entityId}' was not found.") { }
}
