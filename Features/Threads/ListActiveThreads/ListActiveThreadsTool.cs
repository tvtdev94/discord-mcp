namespace DiscordMcp.Features.Threads.ListActiveThreads;

[McpServerToolType]
public sealed class ListActiveThreadsTool(IMediator mediator)
{
    [McpServerTool(Name = "list_active_threads"), Description("List all active threads in the server")]
    public Task<string> ListActiveThreads(
        [Description("Discord server ID (uses default if omitted)")] string? guildId = null)
        => mediator.Send(new ListActiveThreadsQuery(guildId));
}
