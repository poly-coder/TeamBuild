using System.Runtime.CompilerServices;
using DiffEngine;

namespace TeamBuild.Projects.Domain.UnitTests;

internal static class ModuleInitialization
{
    [ModuleInitializer]
    internal static void Init()
    {
        DiffTools.UseOrder(DiffTool.VisualStudioCode);

        VerifierSettings.DontIgnoreEmptyCollections();
    }
}
