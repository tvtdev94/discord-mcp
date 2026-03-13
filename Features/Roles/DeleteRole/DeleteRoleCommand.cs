namespace DiscordMcp.Features.Roles.DeleteRole;

/// <summary>Command to permanently delete a role from a Discord server.</summary>
public record DeleteRoleCommand(string? GuildId, string RoleId) : IRequest<string>;
