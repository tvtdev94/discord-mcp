namespace DiscordMcp.Features.Webhooks.SendWebhookMessage;

/// <summary>Command to send a message via a Discord webhook URL.</summary>
public record SendWebhookMessageCommand(string WebhookUrl, string Message) : IRequest<string>;
