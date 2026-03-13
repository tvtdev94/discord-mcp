namespace DiscordMcp.Features.Messages.DeleteMessage;

/// <summary>Command to delete a message from a Discord channel.</summary>
public record DeleteMessageCommand(string ChannelId, string MessageId) : IRequest<string>;
