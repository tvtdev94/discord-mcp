namespace DiscordMcp.Features.Users.DeletePrivateMessage;

[McpServerToolType]
public sealed class DeletePrivateMessageTool(IMediator mediator)
{
    [McpServerTool(Name = "delete_private_message"), Description("Delete a private message sent to a specific user")]
    public Task<string> DeletePrivateMessage(
        [Description("Discord user ID")] string userId,
        [Description("Message ID to delete")] string messageId)
        => mediator.Send(new DeletePrivateMessageCommand(userId, messageId));
}
