namespace DiscordMcp.Features.Webhooks.DeleteWebhook;

/// <summary>Command to delete a webhook by ID.</summary>
public record DeleteWebhookCommand(string WebhookId) : IRequest<string>;
