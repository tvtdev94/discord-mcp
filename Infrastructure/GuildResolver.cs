namespace DiscordMcp.Infrastructure;

/// <summary>
/// Resolves Discord guild (server) from an optional guildId parameter,
/// falling back to the DISCORD_GUILD_ID environment variable.
/// Shared across all handlers that need guild resolution.
/// </summary>
public static class GuildResolver
{
    private static readonly string DefaultGuildId =
        Environment.GetEnvironmentVariable("DISCORD_GUILD_ID") ?? string.Empty;

    /// <summary>
    /// Returns the effective guild ID: uses the provided value if non-empty,
    /// otherwise falls back to the DISCORD_GUILD_ID environment variable.
    /// </summary>
    public static string ResolveId(string? guildId) =>
        string.IsNullOrWhiteSpace(guildId) && !string.IsNullOrWhiteSpace(DefaultGuildId)
            ? DefaultGuildId
            : guildId ?? string.Empty;

    /// <summary>
    /// Resolves the guild ID and returns the SocketGuild from the Discord client.
    /// Throws ArgumentException if guild ID is missing or guild not found.
    /// </summary>
    public static SocketGuild Resolve(DiscordSocketClient client, string? guildId)
    {
        var resolved = ResolveId(guildId);
        if (string.IsNullOrWhiteSpace(resolved))
            throw new ArgumentException("guildId cannot be null. Provide a guildId or set DISCORD_GUILD_ID environment variable.");

        if (!ulong.TryParse(resolved, out var id))
            throw new ArgumentException($"guildId '{resolved}' is not a valid Discord snowflake ID.");

        return client.GetGuild(id)
            ?? throw new ArgumentException($"Discord server not found for guildId '{resolved}'.");
    }
}
