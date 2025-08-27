using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace TeamBuild.Core.Testing;

public static partial class VerifyExtensions
{
    [GeneratedRegex(@"[\W]", RegexOptions.Compiled)]
    private static partial Regex InvalidRuleCharRegex();

    public static SettingsTask UseRuleName(
        this SettingsTask settingsTask,
        string rule,
        [CallerMemberName] string? methodName = null
    )
    {
        var normalizedRule = InvalidRuleCharRegex().Replace(rule, "_");

        methodName ??= "UnknownMethod";

        return settingsTask.UseMethodName($"{methodName}_{normalizedRule}");
    }

    public static SettingsTask UseSnapshotsDirectory(this SettingsTask settingsTask) =>
        settingsTask.UseDirectory("_snapshots");
}
