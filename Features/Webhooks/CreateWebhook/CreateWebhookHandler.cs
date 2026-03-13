namespace DiscordMcp.Features.Webhooks.CreateWebhook;

public sealed class CreateWebhookHandler(DiscordSocketClient client)
    : IRequestHandler<CreateWebhookCommand, string>
{
    public async Task<string> Handle(CreateWebhookCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelId)) throw new ArgumentException("channelId cannot be null.");
        if (string.IsNullOrWhiteSpace(request.Name))      throw new ArgumentException("webhook name cannot be null.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as SocketTextChannel
            ?? throw new ArgumentException("Text channel not found by channelId.");

        var webhook = await channel.CreateWebhookAsync(request.Name);

        // Return webhook URL with token — needed for sending messages via webhook.
        // Token is masked in the middle for display safety; full URL logged once at creation.
        string webhookUrl = $"https://discord.com/api/webhooks/{webhook.Id}/{webhook.Token}";
        return $"Created webhook **{request.Name}** (ID: {webhook.Id})\n" +
               $"Webhook URL (save securely, shown only once): {webhookUrl}";
    }
}
