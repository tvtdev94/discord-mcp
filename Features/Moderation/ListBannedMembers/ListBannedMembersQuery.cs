namespace DiscordMcp.Features.Moderation.ListBannedMembers;

/// <summary>Query to list all banned members from a Discord server.</summary>
public record ListBannedMembersQuery(string? GuildId) : IRequest<string>;
