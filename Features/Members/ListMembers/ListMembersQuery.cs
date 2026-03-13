namespace DiscordMcp.Features.Members.ListMembers;

/// <summary>Query to list members of a Discord server.</summary>
public record ListMembersQuery(string? GuildId, int Limit) : IRequest<string>;
