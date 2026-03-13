namespace DiscordMcp.Features.Webhooks.ListWebhooks;

/// <summary>Query to list all webhooks on a specific Discord text channel.</summary>
public record ListWebhooksQuery(string ChannelId) : IRequest<string>;
