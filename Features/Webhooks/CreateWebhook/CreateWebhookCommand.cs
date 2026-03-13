namespace DiscordMcp.Features.Webhooks.CreateWebhook;

/// <summary>Command to create a new webhook on a Discord text channel.</summary>
public record CreateWebhookCommand(string ChannelId, string Name) : IRequest<string>;
