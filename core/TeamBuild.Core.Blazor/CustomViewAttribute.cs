namespace TeamBuild.Core.Blazor;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class CustomViewAttribute : Attribute
{
    public string Selector { get; }

    public CustomViewAttribute(string selector)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(selector);
        
        Selector = selector;
    }
}
