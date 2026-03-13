namespace DiscordMcp.Features.Messages.PinMessage;

[McpServerToolType]
public sealed class PinMessageTool(IMediator mediator)
{
    [McpServerTool(Name = "pin_message"), Description("Pin a message in a channel")]
    public Task<string> PinMessage(
        [Description("Channel ID containing the message")] string channelId,
        [Description("Message ID to pin")] string messageId)
        => mediator.Send(new PinMessageCommand(channelId, messageId));
}
