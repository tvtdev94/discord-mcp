namespace DiscordMcp.Features.Channels.FindChannel;

/// <summary>Query to find a channel by name in a Discord server.</summary>
public record FindChannelQuery(string? GuildId, string ChannelName) : IRequest<string>;
