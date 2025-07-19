namespace TeamBuild.Core.Domain;

public class DomainException : Exception
{
    public DomainException() { }

    public DomainException(string message)
        : base(message) { }

    public DomainException(string message, Exception inner)
        : base(message, inner) { }

    public static DomainException UnknownCaseType(string name, object? subject)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var actual =
            subject is null ? "null"
            : subject.GetType() is var type ? type.FullName ?? type.Name
            : "unknown";

        return new UnknownCaseException($"Unknown {name} type: {actual}");
    }

    public static DomainException UnknownCommandType(object command) =>
        UnknownCaseType("command", command);
}

public class UnknownCaseException : DomainException
{
    public UnknownCaseException() { }

    public UnknownCaseException(string message)
        : base(message) { }

    public UnknownCaseException(string message, Exception inner)
        : base(message, inner) { }
}
