namespace DiscordMcp.Infrastructure;

/// <summary>
/// Reads DISCORD_DISABLED_TOOLSETS env var and exposes a fast lookup for
/// whether a given toolset name is disabled.
/// Toolset names map to Features/{Name}/ folder names (lowercase).
/// </summary>
public static class ToolsetFilter
{
    private static readonly HashSet<string> DisabledToolsets = LoadDisabled();

    private static HashSet<string> LoadDisabled()
    {
        var raw = Environment.GetEnvironmentVariable("DISCORD_DISABLED_TOOLSETS") ?? string.Empty;
        return raw
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => s.ToLowerInvariant())
            .ToHashSet();
    }

    /// <summary>
    /// Returns true when the toolset with the given name is disabled.
    /// Name comparison is case-insensitive.
    /// </summary>
    public static bool IsToolsetDisabled(string toolsetName) =>
        DisabledToolsets.Contains(toolsetName.ToLowerInvariant());
}
