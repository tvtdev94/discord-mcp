namespace DiscordMcp.Features.Webhooks.DeleteWebhook;

public sealed class DeleteWebhookHandler(DiscordSocketClient client)
    : IRequestHandler<DeleteWebhookCommand, string>
{
    public async Task<string> Handle(DeleteWebhookCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.WebhookId)) throw new ArgumentException("webhookId cannot be null.");

        // Fetch via REST to get a deletable webhook reference
        var restClient = client.Rest;
        var webhook = await restClient.GetWebhookAsync(SafeParser.ParseUlong(request.WebhookId, "webhookId"))
            ?? throw new ArgumentException("Webhook not found by webhookId.");

        string webhookName = webhook.Name;
        await webhook.DeleteAsync();
        return $"Deleted {webhookName} webhook.";
    }
}
