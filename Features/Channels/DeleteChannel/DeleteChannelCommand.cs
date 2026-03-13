namespace DiscordMcp.Features.Channels.DeleteChannel;

/// <summary>Command to delete a channel from a Discord server.</summary>
public record DeleteChannelCommand(string? GuildId, string ChannelId) : IRequest<string>;
