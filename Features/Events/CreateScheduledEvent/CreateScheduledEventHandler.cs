using Discord;

namespace DiscordMcp.Features.Events.CreateScheduledEvent;

public sealed class CreateScheduledEventHandler(DiscordSocketClient client)
    : IRequestHandler<CreateScheduledEventCommand, string>
{
    public async Task<string> Handle(CreateScheduledEventCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))      throw new ArgumentException("name cannot be null or empty.");
        if (string.IsNullOrWhiteSpace(request.StartTime)) throw new ArgumentException("startTime cannot be null or empty.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        if (!DateTimeOffset.TryParse(request.StartTime, out var startTime))
            throw new ArgumentException($"Invalid startTime format. Use ISO 8601: {request.StartTime}");

        DateTimeOffset? endTime = null;
        if (!string.IsNullOrWhiteSpace(request.EndTime))
        {
            if (!DateTimeOffset.TryParse(request.EndTime, out var parsedEnd))
                throw new ArgumentException($"Invalid endTime format. Use ISO 8601: {request.EndTime}");
            endTime = parsedEnd;
        }

        Discord.Rest.RestGuildEvent createdEvent;

        if (!string.IsNullOrWhiteSpace(request.ChannelId))
        {
            // Voice channel event
            var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as SocketVoiceChannel
                ?? throw new ArgumentException($"Voice channel not found by channelId: {request.ChannelId}");

            createdEvent = await guild.CreateEventAsync(
                name:         request.Name,
                startTime:    startTime,
                type:         GuildScheduledEventType.Voice,
                privacyLevel: GuildScheduledEventPrivacyLevel.Private,
                description:  request.Description,
                endTime:      endTime,
                channelId:    channel.Id);
        }
        else
        {
            // External event — endTime is required
            if (endTime is null)
                throw new ArgumentException("endTime is required for external events (no channelId provided).");

            createdEvent = await guild.CreateEventAsync(
                name:         request.Name,
                startTime:    startTime,
                type:         GuildScheduledEventType.External,
                privacyLevel: GuildScheduledEventPrivacyLevel.Private,
                description:  request.Description,
                endTime:      endTime,
                location:     "TBD");
        }

        return $"Successfully created scheduled event: {createdEvent.Name} (ID: {createdEvent.Id}), starts at {startTime:u}";
    }
}
