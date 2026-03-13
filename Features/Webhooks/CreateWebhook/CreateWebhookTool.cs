namespace DiscordMcp.Features.Webhooks.CreateWebhook;

[McpServerToolType]
public sealed class CreateWebhookTool(IMediator mediator)
{
    [McpServerTool(Name = "create_webhook"), Description("Create a new webhook on a specific channel")]
    public Task<string> CreateWebhook(
        [Description("Discord channel ID")] string channelId,
        [Description("Webhook name")] string name)
        => mediator.Send(new CreateWebhookCommand(channelId, name));
}
