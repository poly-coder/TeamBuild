using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TeamBuild.Core.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string conditionName;

    public RequiredIfAttribute(string conditionName)
        : base(() => "{0} is required under current conditions")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(conditionName);

        this.conditionName = conditionName;
    }

    /// <summary>
    ///     Gets or sets a flag indicating whether the attribute should allow empty strings.
    /// </summary>
    public bool AllowEmptyStrings { get; set; }

    public override bool RequiresValidationContext => true;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (IsConditionMatched(validationContext))
        {
            return IsValidValue(value)
                ? ValidationResult.Success
                : CreateFailedValidationResult(validationContext);
        }

        return ValidationResult.Success;
    }

    private bool IsValidValue(object? value)
    {
        if (value is null)
            return false;

        // only check string length if empty strings are not allowed
        return AllowEmptyStrings
            || value is not string stringValue
            || !string.IsNullOrWhiteSpace(stringValue);
    }

    private bool IsConditionMatched(ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is { } instance && instance.GetType() is var type)
        {
            if (
                type.GetProperty(conditionName, BindingFlags.Public | BindingFlags.Instance)
                    is { CanRead: true } property
                && property.GetIndexParameters() is []
            )
            {
                var conditionValue = property.GetValue(instance);
                return IsConditionTrue(conditionValue);
            }

            if (
                type.GetMethod(conditionName, BindingFlags.Public | BindingFlags.Instance)
                    is { IsGenericMethodDefinition: false } method
                && method.GetParameters() is []
            )
            {
                var conditionValue = method.Invoke(obj: instance, []);
                return IsConditionTrue(conditionValue);
            }

            throw new ArgumentException(
                $"Property '{conditionName}' not found on type '{instance.GetType().FullName}'."
            );
        }

        throw new ArgumentNullException(
            nameof(validationContext),
            "Validation context's object instance cannot be null."
        );
    }

    private bool IsConditionTrue(object? conditionValue)
    {
        if (conditionValue is bool boolValue)
        {
            return boolValue;
        }

        if (conditionValue is string stringValue)
        {
            return !string.IsNullOrWhiteSpace(stringValue);
        }

        return false;
    }

    private ValidationResult CreateFailedValidationResult(ValidationContext validationContext)
    {
        string[]? memberNames = validationContext.MemberName is { } memberName
            ? new[] { memberName }
            : null;

        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
    }
}
