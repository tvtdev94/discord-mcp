namespace DiscordMcp.Features.Messages.ListPinnedMessages;

[McpServerToolType]
public sealed class ListPinnedMessagesTool(IMediator mediator)
{
    [McpServerTool(Name = "list_pinned_messages"), Description("List all pinned messages in a channel")]
    public Task<string> ListPinnedMessages(
        [Description("Channel ID")] string channelId)
        => mediator.Send(new ListPinnedMessagesQuery(channelId));
}
