namespace DiscordMcp.Features.Channels.MoveChannel;

/// <summary>Command to move a channel to a new position index.</summary>
public record MoveChannelCommand(string ChannelId, int Position) : IRequest<string>;
