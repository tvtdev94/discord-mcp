namespace DiscordMcp.Features.Roles.AssignRole;

[McpServerToolType]
public sealed class AssignRoleTool(IMediator mediator)
{
    [McpServerTool(Name = "assign_role"), Description("Assigns a specified role to a user")]
    public Task<string> AssignRole(
        [Description("Discord server ID (uses default if omitted)")] string? guildId,
        [Description("ID of the user to receive the role")] string userId,
        [Description("ID of the role to assign")] string roleId)
        => mediator.Send(new AssignRoleCommand(guildId, userId, roleId));
}
