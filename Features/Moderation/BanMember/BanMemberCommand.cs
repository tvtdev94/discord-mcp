namespace DiscordMcp.Features.Moderation.BanMember;

/// <summary>Command to ban a member from a Discord server.</summary>
public record BanMemberCommand(
    string? GuildId,
    string UserId,
    string? Reason,
    int DeleteMessageDays) : IRequest<string>;
