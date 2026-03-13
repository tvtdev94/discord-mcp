namespace DiscordMcp.Features.Users.SendPrivateMessage;

[McpServerToolType]
public sealed class SendPrivateMessageTool(IMediator mediator)
{
    [McpServerTool(Name = "send_private_message"), Description("Send a private message to a specific user")]
    public Task<string> SendPrivateMessage(
        [Description("Discord user ID")] string userId,
        [Description("Message content")] string message)
        => mediator.Send(new SendPrivateMessageCommand(userId, message));
}
