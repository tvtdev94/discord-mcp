namespace DiscordMcp.Features.Members.GetMemberInfo;

/// <summary>Query to get detailed information about a specific guild member.</summary>
public record GetMemberInfoQuery(string? GuildId, string UserId) : IRequest<string>;
