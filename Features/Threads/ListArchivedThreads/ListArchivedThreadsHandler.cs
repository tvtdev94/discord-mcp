namespace DiscordMcp.Features.Threads.ListArchivedThreads;

public sealed class ListArchivedThreadsHandler(DiscordSocketClient client)
    : IRequestHandler<ListArchivedThreadsQuery, string>
{
    public async Task<string> Handle(ListArchivedThreadsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ChannelId))
            throw new ArgumentException("channelId cannot be null or empty.");

        var channelIdParsed = SafeParser.ParseUlong(request.ChannelId, "channelId");

        // SocketTextChannel does not expose GetPublicArchivedThreadsAsync in Discord.Net 3.x.
        // Retrieve archived threads via the REST guild's thread channel list and filter by parent + archived flag.
        var socketChannel = client.GetChannel(channelIdParsed) as SocketTextChannel
            ?? throw new ArgumentException($"Text channel not found by channelId: {request.ChannelId}");

        var restGuild = await client.Rest.GetGuildAsync(socketChannel.Guild.Id);
        var allThreads = await restGuild.GetThreadChannelsAsync();

        var archived = allThreads
            .Where(t => t.IsArchived && t.ParentChannelId == channelIdParsed)
            .ToList();
        if (archived.Count == 0)
            return $"No archived threads found in #{socketChannel.Name}.";

        var lines = archived.Select(t =>
            $"- {t.Name} (ID: {t.Id})");

        return $"Retrieved {archived.Count} archived thread(s) in #{socketChannel.Name}:\n{string.Join("\n", lines)}";
    }
}
