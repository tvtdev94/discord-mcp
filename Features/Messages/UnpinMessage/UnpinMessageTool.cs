namespace DiscordMcp.Features.Messages.UnpinMessage;

[McpServerToolType]
public sealed class UnpinMessageTool(IMediator mediator)
{
    [McpServerTool(Name = "unpin_message"), Description("Unpin a message in a channel")]
    public Task<string> UnpinMessage(
        [Description("Channel ID containing the message")] string channelId,
        [Description("Message ID to unpin")] string messageId)
        => mediator.Send(new UnpinMessageCommand(channelId, messageId));
}
