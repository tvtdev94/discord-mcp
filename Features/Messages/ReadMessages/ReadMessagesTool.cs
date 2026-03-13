namespace DiscordMcp.Features.Messages.ReadMessages;

[McpServerToolType]
public sealed class ReadMessagesTool(IMediator mediator)
{
    [McpServerTool(Name = "read_messages"), Description("Read recent message history from a specific channel")]
    public Task<string> ReadMessages(
        [Description("Discord channel ID")] string channelId,
        [Description("Number of messages to retrieve (default 100)")] string? count = null)
        => mediator.Send(new ReadMessagesQuery(channelId, count));
}
