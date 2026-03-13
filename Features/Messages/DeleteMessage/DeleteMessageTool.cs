namespace DiscordMcp.Features.Messages.DeleteMessage;

[McpServerToolType]
public sealed class DeleteMessageTool(IMediator mediator)
{
    [McpServerTool(Name = "delete_message"), Description("Delete a message from a specific channel")]
    public Task<string> DeleteMessage(
        [Description("Discord channel ID")] string channelId,
        [Description("Specific message ID")] string messageId)
        => mediator.Send(new DeleteMessageCommand(channelId, messageId));
}
