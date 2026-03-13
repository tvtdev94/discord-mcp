namespace DiscordMcp.Features.Roles.EditRole;

[McpServerToolType]
public sealed class EditRoleTool(IMediator mediator)
{
    [McpServerTool(Name = "edit_role"), Description("Updates settings of an existing role. All parameters except guildId and roleId are optional")]
    public Task<string> EditRole(
        [Description("Discord server ID (uses default if omitted)")] string? guildId,
        [Description("ID of the role to edit")] string roleId,
        [Description("New name for the role")] string? name = null,
        [Description("New color as RGB integer")] string? color = null,
        [Description("New hoist setting")] string? hoist = null,
        [Description("New mentionable setting")] string? mentionable = null,
        [Description("New permissions bitfield as string")] string? permissions = null)
        => mediator.Send(new EditRoleCommand(guildId, roleId, name, color, hoist, mentionable, permissions));
}
