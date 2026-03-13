namespace DiscordMcp.Features.Users.SendPrivateMessage;

/// <summary>Command to send a private (DM) message to a Discord user.</summary>
public record SendPrivateMessageCommand(string UserId, string Message) : IRequest<string>;
