namespace DiscordMcp.Features.Threads.ArchiveThread;

[McpServerToolType]
public sealed class ArchiveThreadTool(IMediator mediator)
{
    [McpServerTool(Name = "archive_thread"), Description("Archive an active Discord thread channel")]
    public Task<string> ArchiveThread(
        [Description("ID of the thread to archive")] string threadId)
        => mediator.Send(new ArchiveThreadCommand(threadId));
}
