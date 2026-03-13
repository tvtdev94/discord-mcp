namespace DiscordMcp.Features.Emojis.ListEmojis;

/// <summary>Query to list all custom emojis in a Discord server.</summary>
public record ListEmojisQuery(string? GuildId) : IRequest<string>;
