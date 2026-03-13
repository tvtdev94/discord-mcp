namespace DiscordMcp.Features.Server.GetServerInfo;

[McpServerToolType]
public sealed class GetServerInfoTool(IMediator mediator)
{
    [McpServerTool(Name = "get_server_info"), Description("Get detailed discord server information")]
    public Task<string> GetServerInfo(
        [Description("Discord server ID (uses default if omitted)")] string? guildId = null)
        => mediator.Send(new GetServerInfoQuery(guildId));
}
