namespace DiscordMcp.Features.Server.EditServer;

[McpServerToolType]
public sealed class EditServerTool(IMediator mediator)
{
    [McpServerTool(Name = "edit_server"), Description("Edit the name and/or description of a Discord server")]
    public Task<string> EditServer(
        [Description("Discord server ID (uses default if omitted)")] string? guildId = null,
        [Description("New server name (optional)")] string? name = null,
        [Description("New server description (optional)")] string? description = null)
        => mediator.Send(new EditServerCommand(guildId, name, description));
}
