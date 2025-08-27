using System.Globalization;

namespace TeamBuild.Core.Domain;

public class DomainException : Exception
{
    public DomainException() { }

    public DomainException(string message)
        : base(message) { }

    public DomainException(string message, Exception inner)
        : base(message, inner) { }
}

public class NotFoundException : DomainException
{
    public NotFoundException() { }

    public NotFoundException(string message)
        : base(message) { }

    public NotFoundException(string message, Exception inner)
        : base(message, inner) { }
}

public class ResourceNotFoundException : NotFoundException
{
    public ResourceNotFoundException(string entityType, string entityId)
        : base(
            string.Format(
                CultureInfo.CurrentCulture,
                LangResources.ResourceNotFound_Message,
                entityType,
                entityId
            )
        ) { }
}
