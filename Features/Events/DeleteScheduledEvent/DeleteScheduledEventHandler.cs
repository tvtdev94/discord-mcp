namespace DiscordMcp.Features.Events.DeleteScheduledEvent;

public sealed class DeleteScheduledEventHandler(DiscordSocketClient client)
    : IRequestHandler<DeleteScheduledEventCommand, string>
{
    public async Task<string> Handle(DeleteScheduledEventCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.EventId)) throw new ArgumentException("eventId cannot be null or empty.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var guildEvent = await guild.GetEventAsync(SafeParser.ParseUlong(request.EventId, "eventId"))
            ?? throw new ArgumentException($"Scheduled event not found by eventId: {request.EventId}");

        var eventName = guildEvent.Name;
        await guildEvent.DeleteAsync();

        return $"Successfully deleted scheduled event: {eventName} (ID: {request.EventId}).";
    }
}
