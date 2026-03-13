namespace DiscordMcp.Features.Messages.EditMessage;

[McpServerToolType]
public sealed class EditMessageTool(IMediator mediator)
{
    [McpServerTool(Name = "edit_message"), Description("Edit a message from a specific channel")]
    public Task<string> EditMessage(
        [Description("Discord channel ID")] string channelId,
        [Description("Specific message ID")] string messageId,
        [Description("New message content")] string newMessage)
        => mediator.Send(new EditMessageCommand(channelId, messageId, newMessage));
}
