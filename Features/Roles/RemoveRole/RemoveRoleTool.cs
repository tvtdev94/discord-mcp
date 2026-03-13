namespace DiscordMcp.Features.Roles.RemoveRole;

[McpServerToolType]
public sealed class RemoveRoleTool(IMediator mediator)
{
    [McpServerTool(Name = "remove_role"), Description("Removes a specified role from a user")]
    public Task<string> RemoveRole(
        [Description("Discord server ID (uses default if omitted)")] string? guildId,
        [Description("ID of the user to remove the role from")] string userId,
        [Description("ID of the role to remove")] string roleId)
        => mediator.Send(new RemoveRoleCommand(guildId, userId, roleId));
}
