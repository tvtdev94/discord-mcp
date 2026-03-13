namespace DiscordMcp.Features.Webhooks.ListWebhooks;

[McpServerToolType]
public sealed class ListWebhooksTool(IMediator mediator)
{
    [McpServerTool(Name = "list_webhooks"), Description("List all webhooks on a specific channel")]
    public Task<string> ListWebhooks(
        [Description("Discord channel ID")] string channelId)
        => mediator.Send(new ListWebhooksQuery(channelId));
}
