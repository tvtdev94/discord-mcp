namespace DiscordMcp.Features.Roles.CreateRole;

/// <summary>Command to create a new role in a Discord server.</summary>
public record CreateRoleCommand(
    string? GuildId,
    string Name,
    string? Color,
    string? Hoist,
    string? Mentionable,
    string? Permissions) : IRequest<string>;
