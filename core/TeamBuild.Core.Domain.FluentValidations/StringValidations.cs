using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using FluentValidation;

namespace TeamBuild.Core.Domain.FluentValidations;

public static class StringValidations
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringValidator Create(StringValidatorOptions? options = null) => new(options);

    #region [ DisplayName ]

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringValidator DisplayNameWith(StringValidatorOptions? options = null) =>
        Create(StringValidatorOptions.DisplayName.Default.ExtendWith(options));

    public static readonly StringValidator DisplayName = Create(
        StringValidatorOptions.DisplayName.Default
    );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(source))]
    public static string? CoerceDisplayName(this string? source)
    {
        return source?.Trim();
    }

    #endregion [ DisplayName ]

    #region [ Summary ]

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringValidator SummaryWith(StringValidatorOptions? options = null) =>
        Create(StringValidatorOptions.Summary.Default.ExtendWith(options));

    public static readonly StringValidator Summary = Create(StringValidatorOptions.Summary.Default);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(source))]
    public static string? CoerceSummary(this string? source)
    {
        return source?.Trim();
    }

    #endregion [ Summary ]

    #region [ Description ]

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringValidator DescriptionWith(StringValidatorOptions? options = null) =>
        Create(StringValidatorOptions.Description.Default.ExtendWith(options));

    public static readonly StringValidator Description = Create(
        StringValidatorOptions.Description.Default
    );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(source))]
    public static string? CoerceDescription(this string? source)
    {
        return source?.Trim();
    }

    #endregion [ Description ]

    #region [ EntityId ]

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringValidator EntityIdWith(StringValidatorOptions? options = null) =>
        Create(StringValidatorOptions.EntityId.Default.ExtendWith(options));

    public static readonly StringValidator EntityId = Create(
        StringValidatorOptions.EntityId.Default
    );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(source))]
    public static string? CoerceEntityId(this string? source)
    {
        return source?.Trim();
    }

    #endregion [ EntityId ]

    #region [ PrefixedEntityId ]

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringValidator PrefixedEntityId(
        string prefix,
        StringValidatorOptions? options = null
    ) => Create(StringValidatorOptions.PrefixedEntityId.Create(prefix).ExtendWith(options));

    #endregion [ PrefixedEntityId ]

    public static class Nullable
    {
        // ReSharper disable MemberHidesStaticFromOuterClass

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static OptionalStringValidator Create(StringValidatorOptions? options = null) =>
            new(options);

        #region [ DisplayName ]

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static OptionalStringValidator DisplayNameWith(
            StringValidatorOptions? options = null
        ) => Create(StringValidatorOptions.DisplayName.Default.ExtendWith(options));

        public static readonly OptionalStringValidator DisplayName = Create(
            StringValidatorOptions.DisplayName.Default
        );

        #endregion [ DisplayName ]

        #region [ Summary ]

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static OptionalStringValidator SummaryWith(StringValidatorOptions? options = null) =>
            Create(StringValidatorOptions.Summary.Default.ExtendWith(options));

        public static readonly OptionalStringValidator Summary = Create(
            StringValidatorOptions.Summary.Default
        );

        #endregion [ Summary ]

        #region [ Description ]

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static OptionalStringValidator DescriptionWith(
            StringValidatorOptions? options = null
        ) => Create(StringValidatorOptions.Description.Default.ExtendWith(options));

        public static readonly OptionalStringValidator Description = Create(
            StringValidatorOptions.Description.Default
        );

        #endregion [ Description ]

        #region [ EntityId ]

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static OptionalStringValidator EntityIdWith(
            StringValidatorOptions? options = null
        ) => Create(StringValidatorOptions.EntityId.Default.ExtendWith(options));

        public static readonly OptionalStringValidator EntityId = Create(
            StringValidatorOptions.EntityId.Default
        );

        #endregion [ EntityId ]

        #region [ PrefixedEntityId ]

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static OptionalStringValidator PrefixedEntityId(
            string prefix,
            StringValidatorOptions? options = null
        ) => Create(StringValidatorOptions.PrefixedEntityId.Create(prefix).ExtendWith(options));

        #endregion [ PrefixedEntityId ]

        // ReSharper restore MemberHidesStaticFromOuterClass
    }
}

public sealed partial record StringValidatorOptions(
    int? MinLength = null,
    int? MaxLength = null,
    Regex? Pattern = null,
    Func<string, string>? PatternMessage = null
)
{
    public StringValidatorOptions ExtendWith(StringValidatorOptions? options)
    {
        if (options == null)
            return this;

        return new StringValidatorOptions(
            MinLength: options.MinLength ?? MinLength,
            MaxLength: options.MaxLength ?? MaxLength,
            Pattern: options.Pattern ?? Pattern,
            PatternMessage: options.PatternMessage ?? PatternMessage
        );
    }

    public override string ToString()
    {
        var data = Tokens().ToArray();

        if (data.Length == 0)
        {
            return "string";
        }

        return $"string {string.Join(", ", data)}";

        IEnumerable<string> Tokens()
        {
            switch ((MinLength, MaxLength))
            {
                case ({ } min, { } max):
                    yield return $"{min}...{max}";
                    break;
                case ({ } min, null):
                    yield return $">={min}";
                    break;
                case (null, { } max):
                    yield return $"<={max}";
                    break;
            }

            if (Pattern != null)
            {
                yield return $"'{Pattern}'";
            }
        }
    }

    #region [ Custom Options ]

    public static readonly StringValidatorOptions Default = new();

    public static class DisplayName
    {
        public const int MaxLength = 60;

        public static readonly StringValidatorOptions Default = StringValidatorOptions.Default with
        {
            MaxLength = MaxLength,
        };
    }

    public static class Summary
    {
        public const int MaxLength = 200;

        public static readonly StringValidatorOptions Default = StringValidatorOptions.Default with
        {
            MaxLength = MaxLength,
        };
    }

    public static class Description
    {
        public const int MaxLength = 4000;

        public static readonly StringValidatorOptions Default = StringValidatorOptions.Default with
        {
            MaxLength = MaxLength,
        };
    }

    public static partial class EntityId
    {
        [StringSyntax(StringSyntaxAttribute.Regex)]
        internal const string CharClass = @"[a-zA-Z0-9_\-]";

        [StringSyntax(StringSyntaxAttribute.Regex)]
        internal const string Pattern = $@"{CharClass}+";

        [GeneratedRegex($@"^{Pattern}$")]
        internal static partial Regex Regex();

        internal const int MaxLength = 64;

        public static readonly StringValidatorOptions Default = StringValidatorOptions.Default with
        {
            MaxLength = MaxLength,
            Pattern = Regex(),
            PatternMessage = _ => ValidationsResources.StringValidation_EntityId_PatternMessage,
        };
    }

    public static partial class PrefixedEntityId
    {
        [StringSyntax(StringSyntaxAttribute.Regex)]
        private const string PrefixPattern = $"{EntityId.CharClass}{{1,16}}";

        [GeneratedRegex($@"^{PrefixPattern}$")]
        private static partial Regex PrefixRegex();

        public static StringValidatorOptions Create(string prefix)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(prefix);

            if (!PrefixRegex().IsMatch(prefix))
            {
                throw new ArgumentException(
                    ValidationsResources.StringValidation_PrefixPattern_ErrorMessage,
                    nameof(prefix)
                );
            }

            var escapedPrefix = Regex.Escape(prefix);

            var regex = new Regex(
                $"^{escapedPrefix}{EntityId.Pattern}$",
                RegexOptions.Compiled | RegexOptions.ExplicitCapture
            );

            string PatternMessage(string _) =>
                string.Format(
                    CultureInfo.InvariantCulture,
                    ValidationsResources.StringValidation_PrefixedEntityId_ErrorMessage,
                    prefix
                );

            return EntityId.Default with
            {
                Pattern = regex,
                PatternMessage = PatternMessage,
            };
        }
    }

    #endregion [ Custom Options ]
}

public interface IStringValidator : IValueValidator<string>
{
    StringValidatorOptions Options { get; }
}

public sealed class StringValidator(StringValidatorOptions? options = null)
    : ValueValidator<string>,
        IStringValidator
{
    public StringValidatorOptions Options { get; } = options ?? StringValidatorOptions.Default;

    public override IRuleBuilder<T, string> ValidationRules<T>(IRuleBuilder<T, string> builder)
    {
        builder = builder.NotEmpty();

        builder = (Options.MinLength, Options.MaxLength) switch
        {
            (null, null) => builder,
            ({ } minLength, null) => builder.MinimumLength(minLength),
            (null, { } maxLength) => builder.MaximumLength(maxLength),
            ({ } minLength, { } maxLength) => builder.Length(minLength, maxLength),
        };

        if (Options.Pattern is not null)
        {
            var builderOptions = builder.Matches(Options.Pattern);
            if (Options.PatternMessage is not null)
            {
                builderOptions = builderOptions.WithMessage(
                    (_, value) => Options.PatternMessage(value)
                );
            }
            builder = builderOptions;
        }

        return builder;
    }

    public override string? ToString()
    {
        return Options.ToString();
    }
}

public interface IOptionalStringValidator : IValueValidator<string?>
{
    StringValidatorOptions Options { get; }
}

public sealed class OptionalStringValidator(StringValidatorOptions? options = null)
    : ValueValidator<string?>,
        IOptionalStringValidator
{
    public StringValidatorOptions Options { get; } = options ?? StringValidatorOptions.Default;

    public override IRuleBuilder<T, string?> ValidationRules<T>(IRuleBuilder<T, string?> builder)
    {
        builder = (Options.MinLength, Options.MaxLength) switch
        {
            (null, null) => builder,
            ({ } minLength, null) => builder.MinimumLength(minLength),
            (null, { } maxLength) => builder.MaximumLength(maxLength),
            ({ } minLength, { } maxLength) => builder.Length(minLength, maxLength),
        };

        if (Options.Pattern != null)
        {
            var builderOptions = builder.Matches(Options.Pattern);
            if (Options.PatternMessage != null)
            {
                builderOptions = builderOptions.WithMessage(
                    (_, value) => Options.PatternMessage(value)
                );
            }
            builder = builderOptions;
        }

        return builder;
    }

    public override string ToString()
    {
        return $"{Options}, optional";
    }
}
