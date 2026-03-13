namespace DiscordMcp.Features.Moderation.GetAuditLog;

/// <summary>Query to retrieve audit log entries from a Discord server.</summary>
public record GetAuditLogQuery(string? GuildId, int Limit) : IRequest<string>;
