namespace DiscordMcp.Features.Events.DeleteScheduledEvent;

/// <summary>Command to delete a scheduled event from a Discord guild.</summary>
public record DeleteScheduledEventCommand(string? GuildId, string EventId) : IRequest<string>;
