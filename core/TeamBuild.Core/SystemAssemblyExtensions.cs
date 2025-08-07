using System.Reflection;
using System.Text.RegularExpressions;

namespace TeamBuild.Core;

public static partial class SystemAssemblyExtensions
{
    /// <summary>
    /// Gets the simple name of the assembly.
    /// </summary>
    /// <param name="assembly">The assembly instance.</param>
    /// <returns>The simple name of the assembly.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the assembly name is null.</exception>
    public static string GetAssemblyName(this Assembly assembly) =>
        assembly.GetName().Name ?? throw new InvalidOperationException("Assembly name is null.");

    /// <summary>
    /// Gets the version of the assembly as a string.
    /// </summary>
    /// <param name="assembly">The assembly instance.</param>
    /// <returns>The version of the assembly as a string.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the assembly version is null.</exception>
    public static string GetAssemblyVersion(this Assembly assembly) =>
        assembly.GetName().Version?.ToString()?.ExtractGroup(AssemblyVersionRegex(), "ver")
        ?? throw new InvalidOperationException("Assembly version is null.");

    [GeneratedRegex("""(?<ver>\d+\.\d+\.\d+)""")]
    private static partial Regex AssemblyVersionRegex();
}
