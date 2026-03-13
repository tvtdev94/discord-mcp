namespace DiscordMcp.Features.Users.DeletePrivateMessage;

/// <summary>Command to delete a private (DM) message sent to a Discord user.</summary>
public record DeletePrivateMessageCommand(string UserId, string MessageId) : IRequest<string>;
