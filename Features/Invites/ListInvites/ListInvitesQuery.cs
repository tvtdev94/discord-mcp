namespace DiscordMcp.Features.Invites.ListInvites;

/// <summary>Query to list all active invites in a Discord guild.</summary>
public record ListInvitesQuery(string? GuildId) : IRequest<string>;
