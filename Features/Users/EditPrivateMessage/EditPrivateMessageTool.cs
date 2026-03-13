namespace DiscordMcp.Features.Users.EditPrivateMessage;

[McpServerToolType]
public sealed class EditPrivateMessageTool(IMediator mediator)
{
    [McpServerTool(Name = "edit_private_message"), Description("Edit a private message sent to a specific user")]
    public Task<string> EditPrivateMessage(
        [Description("Discord user ID")] string userId,
        [Description("Message ID to edit")] string messageId,
        [Description("New message content")] string newMessage)
        => mediator.Send(new EditPrivateMessageCommand(userId, messageId, newMessage));
}
