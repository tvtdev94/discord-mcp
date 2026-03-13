namespace DiscordMcp.Features.Threads.CreateThread;

[McpServerToolType]
public sealed class CreateThreadTool(IMediator mediator)
{
    [McpServerTool(Name = "create_thread"), Description("Create a new public thread in a text channel")]
    public Task<string> CreateThread(
        [Description("ID of the text channel to create the thread in")] string channelId,
        [Description("Name of the thread")] string name,
        [Description("Optional starter message sent into the thread after creation")] string? message = null)
        => mediator.Send(new CreateThreadCommand(channelId, name, message));
}
