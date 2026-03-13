namespace DiscordMcp.Features.Roles.ListRoles;

[McpServerToolType]
public sealed class ListRolesTool(IMediator mediator)
{
    [McpServerTool(Name = "list_roles"), Description("Returns a list of all roles on the server along with their ID, names, colors, positions, and permissions")]
    public Task<string> ListRoles(
        [Description("Discord server ID (uses default if omitted)")] string? guildId = null)
        => mediator.Send(new ListRolesQuery(guildId));
}
