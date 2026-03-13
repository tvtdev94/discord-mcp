namespace DiscordMcp.Features.Moderation.KickMember;

/// <summary>Command to kick a member from a Discord server.</summary>
public record KickMemberCommand(string? GuildId, string UserId, string? Reason) : IRequest<string>;
