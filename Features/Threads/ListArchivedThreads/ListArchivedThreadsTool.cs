namespace DiscordMcp.Features.Threads.ListArchivedThreads;

[McpServerToolType]
public sealed class ListArchivedThreadsTool(IMediator mediator)
{
    [McpServerTool(Name = "list_archived_threads"), Description("List all publicly archived threads in a Discord text channel")]
    public Task<string> ListArchivedThreads(
        [Description("ID of the text channel to list archived threads from")] string channelId)
        => mediator.Send(new ListArchivedThreadsQuery(channelId));
}
