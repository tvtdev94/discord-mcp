namespace DiscordMcp.Features.Users.ReadPrivateMessages;

/// <summary>Query to read recent DM history with a specific Discord user.</summary>
public record ReadPrivateMessagesQuery(string UserId, string? Count) : IRequest<string>;
