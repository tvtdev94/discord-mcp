namespace DiscordMcp.Features.Messages.PinMessage;

/// <summary>Command to pin a message in a channel.</summary>
public record PinMessageCommand(string ChannelId, string MessageId) : IRequest<string>;
