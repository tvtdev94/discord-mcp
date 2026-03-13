namespace DiscordMcp.Features.Roles.CreateRole;

[McpServerToolType]
public sealed class CreateRoleTool(IMediator mediator)
{
    [McpServerTool(Name = "create_role"), Description("Creates a new role on the server with specified parameters")]
    public Task<string> CreateRole(
        [Description("Discord server ID (uses default if omitted)")] string? guildId,
        [Description("Name of the new role")] string name,
        [Description("Color as RGB integer (e.g. 16711680 for red). Default 0")] string? color = null,
        [Description("Whether to display the role separately in the sidebar. Default false")] string? hoist = null,
        [Description("Whether the role can be @mentioned. Default false")] string? mentionable = null,
        [Description("Permissions bitfield as string. Default 0")] string? permissions = null)
        => mediator.Send(new CreateRoleCommand(guildId, name, color, hoist, mentionable, permissions));
}
