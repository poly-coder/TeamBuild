using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using TeamBuild.Core.Testing;

namespace TeamBuild.Core.Domain.Testing;

public static partial class VerifyDomainExtensions
{
    public static SettingsTask Verify(
        this JsonSerializerContext jsonSerializerContext,
        JsonSerializationTestData testData,
        JsonSerializerOptions? defaultOptions = null,
        VerifySettings? settings = null,
        [CallerFilePath] string sourceFile = "",
        [CallerMemberName] string methodName = ""
    )
    {
        var jsonOptions = new JsonSerializerOptions(defaultOptions ?? JsonSerializerOptions.Default)
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            TypeInfoResolverChain = { jsonSerializerContext },
        };

        var json = JsonSerializer.Serialize(testData.Input, jsonOptions);

        var output = JsonSerializer.Deserialize(json, testData.InputType, jsonOptions);

        return Verifier
            .Verify(
                new
                {
                    input = testData.Input,
                    output,
                    json,
                },
                settings: settings,
                // ReSharper disable once ExplicitCallerInfoArgument
                sourceFile: sourceFile
            )
            .UseSnapshotsDirectory()
            .UseRuleName(testData.Rule, methodName: methodName);
    }
}
