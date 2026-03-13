namespace DiscordMcp.Features.Roles.ListRoles;

/// <summary>Query to list all roles in a Discord server.</summary>
public record ListRolesQuery(string? GuildId) : IRequest<string>;
