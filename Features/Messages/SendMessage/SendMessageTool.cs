namespace DiscordMcp.Features.Messages.SendMessage;

[McpServerToolType]
public sealed class SendMessageTool(IMediator mediator)
{
    [McpServerTool(Name = "send_message"), Description("Send a message to a specific channel")]
    public Task<string> SendMessage(
        [Description("Discord channel ID")] string channelId,
        [Description("Message content")] string message)
        => mediator.Send(new SendMessageCommand(channelId, message));
}
