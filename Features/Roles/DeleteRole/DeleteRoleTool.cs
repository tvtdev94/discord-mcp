namespace DiscordMcp.Features.Roles.DeleteRole;

[McpServerToolType]
public sealed class DeleteRoleTool(IMediator mediator)
{
    [McpServerTool(Name = "delete_role"), Description("Permanently deletes a role from the server")]
    public Task<string> DeleteRole(
        [Description("Discord server ID (uses default if omitted)")] string? guildId,
        [Description("ID of the role to delete")] string roleId)
        => mediator.Send(new DeleteRoleCommand(guildId, roleId));
}
