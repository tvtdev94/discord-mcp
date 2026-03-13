namespace DiscordMcp.Features.Users.EditPrivateMessage;

/// <summary>Command to edit a private (DM) message sent to a Discord user.</summary>
public record EditPrivateMessageCommand(string UserId, string MessageId, string NewMessage) : IRequest<string>;
