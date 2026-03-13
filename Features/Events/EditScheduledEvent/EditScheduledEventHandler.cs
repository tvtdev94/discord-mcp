using Discord;

namespace DiscordMcp.Features.Events.EditScheduledEvent;

public sealed class EditScheduledEventHandler(DiscordSocketClient client)
    : IRequestHandler<EditScheduledEventCommand, string>
{
    public async Task<string> Handle(EditScheduledEventCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.EventId)) throw new ArgumentException("eventId cannot be null or empty.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var guildEvent = await guild.GetEventAsync(SafeParser.ParseUlong(request.EventId, "eventId"))
            ?? throw new ArgumentException($"Scheduled event not found by eventId: {request.EventId}");

        DateTimeOffset? startTime = null;
        if (!string.IsNullOrWhiteSpace(request.StartTime))
        {
            if (!DateTimeOffset.TryParse(request.StartTime, out var parsed))
                throw new ArgumentException($"Invalid startTime format. Use ISO 8601: {request.StartTime}");
            startTime = parsed;
        }

        DateTimeOffset? endTime = null;
        if (!string.IsNullOrWhiteSpace(request.EndTime))
        {
            if (!DateTimeOffset.TryParse(request.EndTime, out var parsed))
                throw new ArgumentException($"Invalid endTime format. Use ISO 8601: {request.EndTime}");
            endTime = parsed;
        }

        await guildEvent.ModifyAsync(x =>
        {
            if (!string.IsNullOrWhiteSpace(request.Name))        x.Name        = request.Name;
            if (!string.IsNullOrWhiteSpace(request.Description)) x.Description = request.Description;
            if (startTime is not null)                            x.StartTime   = startTime.Value;
            if (endTime is not null)                              x.EndTime     = endTime.Value;
        });

        return $"Successfully updated scheduled event (ID: {guildEvent.Id}).";
    }
}
