namespace DiscordMcp.Features.Channels.ListChannels;

/// <summary>Query to list all channels in a Discord server.</summary>
public record ListChannelsQuery(string? GuildId) : IRequest<string>;
