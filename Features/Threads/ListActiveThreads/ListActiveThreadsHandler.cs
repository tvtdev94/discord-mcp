using Discord;

namespace DiscordMcp.Features.Threads.ListActiveThreads;

public sealed class ListActiveThreadsHandler(DiscordSocketClient client)
    : IRequestHandler<ListActiveThreadsQuery, string>
{
    public async Task<string> Handle(ListActiveThreadsQuery request, CancellationToken cancellationToken)
    {
        var guild = GuildResolver.Resolve(client, request.GuildId);

        // Aggregate active threads from each text channel; de-duplicate by ID
        var allThreads = new List<IThreadChannel>();
        foreach (var textChannel in guild.TextChannels)
        {
            try
            {
                var channelThreads = await textChannel.GetActiveThreadsAsync();
                allThreads.AddRange(channelThreads);
            }
            catch (Exception)
            {
                // Skip channels the bot cannot access (e.g. missing permissions)
            }
        }

        var threads = allThreads
            .GroupBy(t => t.Id)
            .Select(g => g.First())
            .ToList();

        if (threads.Count == 0)
            return "No active threads found in the server.";

        var lines = threads.Select(t =>
        {
            string parentName = t is SocketThreadChannel stc ? stc.ParentChannel?.Name ?? "unknown" : "unknown";
            string archived   = t.IsArchived ? " (archived)" : string.Empty;
            return $"- {t.Name} (ID: {t.Id}) in #{parentName}{archived}";
        });

        return $"Retrieved {threads.Count} active threads:\n{string.Join("\n", lines)}";
    }
}
