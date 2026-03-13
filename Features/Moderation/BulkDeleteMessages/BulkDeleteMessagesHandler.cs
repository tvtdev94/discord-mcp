using Discord;

namespace DiscordMcp.Features.Moderation.BulkDeleteMessages;

public sealed class BulkDeleteMessagesHandler(DiscordSocketClient client)
    : IRequestHandler<BulkDeleteMessagesCommand, string>
{
    // Discord bulk delete only works for messages younger than 14 days
    private static readonly TimeSpan MaxMessageAge = TimeSpan.FromDays(14);

    public async Task<string> Handle(BulkDeleteMessagesCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelId))
            throw new ArgumentException("channelId cannot be null.");
        if (request.Count < 2 || request.Count > 100)
            throw new ArgumentException("count must be between 2 and 100.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as ITextChannel
            ?? throw new ArgumentException("Text channel not found by channelId.");

        var messages = await channel.GetMessagesAsync(request.Count).FlattenAsync();

        // Discord bulk delete API only accepts messages less than 14 days old
        var eligible = messages
            .Where(m => DateTimeOffset.UtcNow - m.CreatedAt < MaxMessageAge)
            .ToList();

        if (eligible.Count == 0)
            return "No eligible messages found. Bulk delete only works for messages less than 14 days old.";

        await channel.DeleteMessagesAsync(eligible);
        return $"Bulk deleted {eligible.Count} message(s) from channel {request.ChannelId}.";
    }
}
