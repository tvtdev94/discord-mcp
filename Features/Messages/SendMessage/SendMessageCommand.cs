namespace DiscordMcp.Features.Messages.SendMessage;

/// <summary>Command to send a message to a Discord channel.</summary>
public record SendMessageCommand(string ChannelId, string Message) : IRequest<string>;
