using Discord.Webhook;

namespace DiscordMcp.Features.Webhooks.SendWebhookMessage;

public sealed class SendWebhookMessageHandler
    : IRequestHandler<SendWebhookMessageCommand, string>
{
    public async Task<string> Handle(SendWebhookMessageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.WebhookUrl)) throw new ArgumentException("webhookUrl cannot be null.");
        if (string.IsNullOrWhiteSpace(request.Message))    throw new ArgumentException("message cannot be null.");

        // Validate URL points to Discord webhook endpoint to prevent SSRF
        if (!Uri.TryCreate(request.WebhookUrl, UriKind.Absolute, out var uri)
            || uri.Scheme != "https"
            || !uri.Host.Equals("discord.com", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("webhookUrl must be a valid Discord webhook URL (https://discord.com/api/webhooks/...).");

        using var webhookClient = new DiscordWebhookClient(request.WebhookUrl);
        var messageId = await webhookClient.SendMessageAsync(request.Message);
        return $"Message sent successfully via webhook. Message ID: {messageId}";
    }
}
