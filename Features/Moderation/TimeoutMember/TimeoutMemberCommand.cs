namespace DiscordMcp.Features.Moderation.TimeoutMember;

/// <summary>Command to apply a timeout to a Discord server member.</summary>
public record TimeoutMemberCommand(
    string? GuildId,
    string UserId,
    int DurationMinutes,
    string? Reason) : IRequest<string>;
