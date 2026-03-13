namespace DiscordMcp.Features.Moderation.UnbanMember;

/// <summary>Command to unban a member from a Discord server.</summary>
public record UnbanMemberCommand(string? GuildId, string UserId) : IRequest<string>;
