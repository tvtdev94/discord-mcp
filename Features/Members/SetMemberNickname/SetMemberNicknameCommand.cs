namespace DiscordMcp.Features.Members.SetMemberNickname;

/// <summary>Command to set or reset a member's nickname in a Discord server.</summary>
public record SetMemberNicknameCommand(string? GuildId, string UserId, string? Nickname) : IRequest<string>;
