namespace DiscordMcp.Features.Messages.UnpinMessage;

/// <summary>Command to unpin a message in a channel.</summary>
public record UnpinMessageCommand(string ChannelId, string MessageId) : IRequest<string>;
