namespace DiscordMcp.Features.Moderation.BulkDeleteMessages;

[McpServerToolType]
public sealed class BulkDeleteMessagesTool(IMediator mediator)
{
    [McpServerTool(Name = "bulk_delete_messages"), Description("Bulk delete messages from a Discord channel (max 100, messages must be under 14 days old)")]
    public Task<string> BulkDeleteMessages(
        [Description("Discord channel ID")] string channelId,
        [Description("Number of messages to delete (2-100)")] int count)
        => mediator.Send(new BulkDeleteMessagesCommand(channelId, count));
}
