namespace DiscordMcp.Features.Webhooks.DeleteWebhook;

[McpServerToolType]
public sealed class DeleteWebhookTool(IMediator mediator)
{
    [McpServerTool(Name = "delete_webhook"), Description("Delete a webhook")]
    public Task<string> DeleteWebhook(
        [Description("Discord webhook ID")] string webhookId)
        => mediator.Send(new DeleteWebhookCommand(webhookId));
}
