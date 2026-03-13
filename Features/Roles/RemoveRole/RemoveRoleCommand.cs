namespace DiscordMcp.Features.Roles.RemoveRole;

/// <summary>Command to remove a role from a guild member.</summary>
public record RemoveRoleCommand(string? GuildId, string UserId, string RoleId) : IRequest<string>;
