using System.Text.Json.Serialization;

namespace TeamBuild.Projects.Domain.CultureFeature;

// Queries
[JsonSerializable(typeof(AvailableCultureQuery))]
[JsonSerializable(typeof(AvailableCultureQuerySuccess))]
[JsonSerializable(typeof(CultureQuery))]
[JsonSerializable(typeof(CultureQuerySuccess))]
// Commands
[JsonSerializable(typeof(CultureCommand))]
[JsonSerializable(typeof(CultureCommandSuccess))]
public partial class CultureJsonContext : JsonSerializerContext { }
