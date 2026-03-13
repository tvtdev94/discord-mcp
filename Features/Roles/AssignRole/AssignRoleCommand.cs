namespace DiscordMcp.Features.Roles.AssignRole;

/// <summary>Command to assign a role to a guild member.</summary>
public record AssignRoleCommand(string? GuildId, string UserId, string RoleId) : IRequest<string>;
