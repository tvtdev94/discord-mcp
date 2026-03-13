using Discord;
using System.Text;

namespace DiscordMcp.Features.Messages.ListPinnedMessages;

public sealed class ListPinnedMessagesHandler(DiscordSocketClient client)
    : IRequestHandler<ListPinnedMessagesQuery, string>
{
    private const int PreviewLength = 100;

    public async Task<string> Handle(ListPinnedMessagesQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelId))
            throw new ArgumentException("channelId cannot be null.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as IMessageChannel
            ?? throw new ArgumentException("Message channel not found by channelId.");

        var pinned = await channel.GetPinnedMessagesAsync();
        var messages = pinned.ToList();

        if (messages.Count == 0)
            return $"No pinned messages found in channel (ID: {request.ChannelId}).";

        var sb = new StringBuilder();
        sb.AppendLine($"Pinned messages in channel (ID: {request.ChannelId}) — {messages.Count} total:");

        foreach (var msg in messages)
        {
            var author  = msg.Author?.Username ?? "unknown";
            var preview = msg.Content?.Length > PreviewLength
                ? msg.Content[..PreviewLength] + "..."
                : msg.Content ?? "(no text content)";
            var timestamp = msg.Timestamp.UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            sb.AppendLine($"  [{timestamp}] @{author} (ID: {msg.Id}): {preview}");
        }

        return sb.ToString().TrimEnd();
    }
}
