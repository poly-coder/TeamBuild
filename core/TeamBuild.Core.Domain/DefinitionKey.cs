using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace TeamBuild.Core.Domain;

public enum DefinitionKeyPartType
{
    Module,
    Resource,
    Operation,
}

public sealed partial class DefinitionKeyPart
{
    public const char Separator = ':';

    private DefinitionKeyPart(DefinitionKeyPartType type, string name)
    {
        Type = type;
        Name = name;
        value = $"{Type.ToShortString()}{Separator}{name}";
    }

    public DefinitionKeyPartType Type { get; }
    public string Name { get; }
    private readonly string value;

    public void Deconstruct(out DefinitionKeyPartType type, out string name)
    {
        type = Type;
        name = Name;
    }

    [StringSyntax(StringSyntaxAttribute.Regex)]
    internal const string TypePattern = @"[A-Z]";

    [StringSyntax(StringSyntaxAttribute.Regex)]
    internal const string NamePattern = @"([a-z][a-z0-9]*)([\-_][a-z0-9]+)*";

    [StringSyntax(StringSyntaxAttribute.Regex)]
    internal const string PartPattern =
        @"(?<type>[A-Z]):(?<name>([a-z][a-z0-9]*)([\-_][a-z0-9]+)*)";

    [GeneratedRegex($@"^{NamePattern}$", RegexOptions.ExplicitCapture)]
    internal static partial Regex NameRegex();

    [GeneratedRegex($@"^{PartPattern}$", RegexOptions.ExplicitCapture)]
    internal static partial Regex PartRegex();

    private bool Equals(DefinitionKeyPart other)
    {
        return Type == other.Type && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((DefinitionKeyPart)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Type, Name);
    }

    public override string ToString() => value;

#pragma warning disable S3875
    public static bool operator ==(DefinitionKeyPart? left, DefinitionKeyPart? right) =>
        ReferenceEquals(left, right) || (left is not null && left.Equals((object?)right));
#pragma warning restore S3875

    public static bool operator !=(DefinitionKeyPart? left, DefinitionKeyPart? right) =>
        !(left == right);

    public static implicit operator string(DefinitionKeyPart part) => part.ToString();

    public static explicit operator DefinitionKeyPart(string text) => Parse(text);

    public static bool TryCreate(
        DefinitionKeyPartType type,
        string name,
        [NotNullWhen(true)] out DefinitionKeyPart? part
    )
    {
        if (name.IsNullOrWhiteSpace() || !NameRegex().IsMatch(name) || !Enum.IsDefined(type))
        {
            part = null;
            return false;
        }

        part = new DefinitionKeyPart(type, name);
        return true;
    }

    public static DefinitionKeyPart Create(DefinitionKeyPartType type, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!Enum.IsDefined(type))
        {
            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        if (!NameRegex().IsMatch(name))
        {
            throw new ArgumentException(
                LangResources.DefinitionKeyPart_InvalidNameMessage,
                nameof(name)
            );
        }

        return new DefinitionKeyPart(type, name);
    }

    public static bool TryParse(string text, [NotNullWhen(true)] out DefinitionKeyPart? part)
    {
        part = null;

        if (text.IsNullOrWhiteSpace())
            return false;

        var match = PartRegex().Match(text);

        if (!match.Success)
            return false;

        var type = match.Groups["type"].Value.AsSpan().ToDefinitionKeyPartType();

        if (!Enum.IsDefined(type))
            return false;

        var name = match.Groups["name"].Value;

        return TryCreate(type, name, out part);
    }

    public static DefinitionKeyPart Parse(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        var match = PartRegex().Match(text);

        if (!match.Success)
        {
            throw new ArgumentException(
                LangResources.DefinitionKeyPart_InvalidFormatMessage,
                nameof(text)
            );
        }

        var type = match.Groups["type"].Value.AsSpan().ToDefinitionKeyPartType();

        if (!Enum.IsDefined(type))
        {
            throw new ArgumentException(
                LangResources.DefinitionKeyPart_InvalidFormatMessage,
                nameof(text)
            );
        }

        var name = match.Groups["name"].Value;

        return Create(type, name);
    }
}

public partial class DefinitionKey
{
    public const char Separator = '.';

    private DefinitionKey(IReadOnlyList<DefinitionKeyPart> parts)
    {
        Parts = parts;
        Id = ComputeId(parts);
        FullName = ComputeFullName(parts);
    }

    public IReadOnlyList<DefinitionKeyPart> Parts { get; }
    public string Id { get; }
    public string Name => Part.Name;
    public string FullName { get; }
    public DefinitionKeyPart Part => Parts[^1];

    [StringSyntax(StringSyntaxAttribute.Regex)]
    internal const string KeyPattern = @$"((?<part>{DefinitionKeyPart.PartPattern})\.)+";

    [GeneratedRegex($@"^{KeyPattern}$", RegexOptions.ExplicitCapture)]
    internal static partial Regex KeyRegex();

    private bool Equals(DefinitionKey other)
    {
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((DefinitionKey)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }

    public override string ToString() => Id;

    private static string ComputeId(IReadOnlyList<DefinitionKeyPart> parts)
    {
        var sb = new StringBuilder();
        foreach (var part in parts)
        {
            sb.Append(part);
            sb.Append(Separator);
        }
        return sb.ToString();
    }

    private static string ComputeFullName(IReadOnlyList<DefinitionKeyPart> parts)
    {
        var sb = new StringBuilder();
        foreach (var part in parts)
        {
            if (sb.Length > 0)
                sb.Append(Separator);
            sb.Append(part.Name);
        }
        return sb.ToString();
    }

#pragma warning disable S3875
    public static bool operator ==(DefinitionKey? left, DefinitionKey? right) =>
        ReferenceEquals(left, right) || (left is not null && left.Equals((object?)right));
#pragma warning restore S3875

    public static bool operator !=(DefinitionKey? left, DefinitionKey? right) => !(left == right);

    public static implicit operator string(DefinitionKey part) => part.ToString();

    public static explicit operator DefinitionKey(string text) => Parse(text);

    public static bool TryCreate(
        IEnumerable<DefinitionKeyPart> parts,
        [NotNullWhen(true)] out DefinitionKey? key
    )
    {
        key = null;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (parts is null)
        {
            return false;
        }

        var partList = parts.ToList().AsReadOnly();

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (partList.Any(p => p is null))
        {
            return false;
        }

        key = new DefinitionKey(partList);
        return true;
    }

    public static DefinitionKey Create(IEnumerable<DefinitionKeyPart> parts)
    {
        ArgumentNullException.ThrowIfNull(parts);

        var partList = parts.ToList().AsReadOnly();

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (partList.Any(p => p is null))
            throw new ArgumentException(
                LangResources.DefinitionKey_InvalidPartsMessage,
                nameof(parts)
            );

        return new DefinitionKey(partList);
    }

    public static bool TryParse(string text, [NotNullWhen(true)] out DefinitionKey? key)
    {
        key = null;

        if (text.IsNullOrWhiteSpace())
        {
            return false;
        }

        var match = KeyRegex().Match(text);

        if (!match.Success)
        {
            return false;
        }

        var parts = new List<DefinitionKeyPart>();

        foreach (var partStr in match.Groups["part"].Captures.Cast<Capture>())
        {
            if (!DefinitionKeyPart.TryParse(partStr.Value, out var part))
            {
                return false;
            }
            parts.Add(part);
        }

        return TryCreate(parts, out key);
    }

    public static DefinitionKey Parse(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        if (!KeyRegex().IsMatch(text))
        {
            throw new ArgumentException(
                LangResources.DefinitionKey_InvalidPartsMessage,
                nameof(text)
            );
        }

        var parts = new List<DefinitionKeyPart>();

        foreach (var partStr in KeyRegex().Match(text).Groups["part"].Captures.Cast<Capture>())
        {
            parts.Add(DefinitionKeyPart.Parse(partStr.Value));
        }

        return Create(parts);
    }

    public static DefinitionKey Module(string name) =>
        Create([DefinitionKeyPart.Create(DefinitionKeyPartType.Module, name)]);

    public DefinitionKey Resource(string name) =>
        Create(Parts.Append(DefinitionKeyPart.Create(DefinitionKeyPartType.Resource, name)));

    public DefinitionKey Operation(string name) =>
        Create(Parts.Append(DefinitionKeyPart.Create(DefinitionKeyPartType.Operation, name)));
}

public static partial class DefinitionKeyExtensions
{
    internal static string ToShortString(this DefinitionKeyPartType type) =>
        type switch
        {
            DefinitionKeyPartType.Module => "M",
            DefinitionKeyPartType.Resource => "R",
            DefinitionKeyPartType.Operation => "O",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };

    internal static DefinitionKeyPartType ToDefinitionKeyPartType(this ReadOnlySpan<char> type)
    {
        return type.Length switch
        {
            1 => type[0] switch
            {
                'M' => DefinitionKeyPartType.Module,
                'R' => DefinitionKeyPartType.Resource,
                'O' => DefinitionKeyPartType.Operation,
                _ => (DefinitionKeyPartType)(-1),
            },
            _ => (DefinitionKeyPartType)(-1),
        };
    }
}
