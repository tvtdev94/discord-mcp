namespace DiscordMcp.Features.Roles.EditRole;

/// <summary>Command to update settings of an existing Discord role.</summary>
public record EditRoleCommand(
    string? GuildId,
    string RoleId,
    string? Name,
    string? Color,
    string? Hoist,
    string? Mentionable,
    string? Permissions) : IRequest<string>;
