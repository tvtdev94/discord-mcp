using Discord;

namespace DiscordMcp.Features.Events.ListScheduledEvents;

public sealed class ListScheduledEventsHandler(DiscordSocketClient client)
    : IRequestHandler<ListScheduledEventsQuery, string>
{
    public async Task<string> Handle(ListScheduledEventsQuery request, CancellationToken cancellationToken)
    {
        var guild = GuildResolver.Resolve(client, request.GuildId);

        var events = await guild.GetEventsAsync();
        var eventList = events.ToList();

        if (eventList.Count == 0)
            return "No scheduled events found in the server.";

        var lines = eventList.Select(e =>
        {
            var status = e.Status switch
            {
                GuildScheduledEventStatus.Scheduled => "Scheduled",
                GuildScheduledEventStatus.Active    => "Active",
                GuildScheduledEventStatus.Completed => "Completed",
                GuildScheduledEventStatus.Cancelled => "Cancelled",
                _                                   => "Unknown"
            };
            var endInfo = e.EndTime.HasValue ? $" → {e.EndTime.Value:u}" : string.Empty;
            return $"- [{status}] {e.Name} (ID: {e.Id}) | Starts: {e.StartTime:u}{endInfo}";
        });

        return $"Retrieved {eventList.Count} scheduled event(s):\n{string.Join("\n", lines)}";
    }
}
