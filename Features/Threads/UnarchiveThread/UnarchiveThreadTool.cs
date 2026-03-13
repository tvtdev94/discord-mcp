namespace DiscordMcp.Features.Threads.UnarchiveThread;

[McpServerToolType]
public sealed class UnarchiveThreadTool(IMediator mediator)
{
    [McpServerTool(Name = "unarchive_thread"), Description("Unarchive a previously archived Discord thread channel")]
    public Task<string> UnarchiveThread(
        [Description("ID of the thread to unarchive")] string threadId)
        => mediator.Send(new UnarchiveThreadCommand(threadId));
}
