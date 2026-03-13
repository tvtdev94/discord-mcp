namespace DiscordMcp.Features.Events.ListScheduledEvents;

/// <summary>Query to list all scheduled events in a Discord guild.</summary>
public record ListScheduledEventsQuery(string? GuildId) : IRequest<string>;
