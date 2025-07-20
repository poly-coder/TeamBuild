namespace TeamBuild.Core.Domain;

public class DomainException : Exception
{
    public DomainException() { }

    public DomainException(string message)
        : base(message) { }

    public DomainException(string message, Exception inner)
        : base(message, inner) { }

    public static DomainException UnexpectedCaseType(string name, object? subject)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var actual =
            subject is null ? "null"
            : subject.GetType() is var type ? type.FullName ?? type.Name
            : "unknown";

        return new UnexpectedCaseException($"Unexpected {name} type: {actual}");
    }

    public static DomainException UnexpectedCommandType(object command) =>
        UnexpectedCaseType("command", command);
}

public class UnexpectedCaseException : DomainException
{
    public UnexpectedCaseException() { }

    public UnexpectedCaseException(string message)
        : base(message) { }

    public UnexpectedCaseException(string message, Exception inner)
        : base(message, inner) { }
}
