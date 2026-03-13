namespace DiscordMcp.Features.Moderation.RemoveTimeout;

/// <summary>Command to remove a timeout from a Discord server member.</summary>
public record RemoveTimeoutCommand(string? GuildId, string UserId) : IRequest<string>;
