namespace DiscordMcp.Features.Webhooks.SendWebhookMessage;

[McpServerToolType]
public sealed class SendWebhookMessageTool(IMediator mediator)
{
    [McpServerTool(Name = "send_webhook_message"), Description("Send a message via webhook URL")]
    public Task<string> SendWebhookMessage(
        [Description("Discord webhook URL")] string webhookUrl,
        [Description("Message content")] string message)
        => mediator.Send(new SendWebhookMessageCommand(webhookUrl, message));
}
