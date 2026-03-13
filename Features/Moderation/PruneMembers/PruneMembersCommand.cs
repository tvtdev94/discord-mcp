namespace DiscordMcp.Features.Moderation.PruneMembers;

/// <summary>Command to prune inactive members from a Discord server.</summary>
public record PruneMembersCommand(string? GuildId, int Days) : IRequest<string>;
