namespace DiscordMcp.Features.Webhooks.ListWebhooks;

public sealed class ListWebhooksHandler(DiscordSocketClient client)
    : IRequestHandler<ListWebhooksQuery, string>
{
    public async Task<string> Handle(ListWebhooksQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelId)) throw new ArgumentException("channelId cannot be null.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as SocketTextChannel
            ?? throw new ArgumentException("Text channel not found by channelId.");

        var webhooks = (await channel.GetWebhooksAsync()).ToList();
        if (webhooks.Count == 0)
            return "No webhooks found for this channel.";

        // Do not expose webhook tokens in list output — tokens are secrets
        var lines = webhooks.Select(w =>
            $"- (ID: {w.Id}) **{w.Name}** | Creator: {w.Creator?.Username ?? "unknown"}");

        return $"**Retrieved {webhooks.Count} webhooks:**\n{string.Join("\n", lines)}";
    }
}
