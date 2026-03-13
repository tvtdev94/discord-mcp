namespace DiscordMcp.Features.Events.EditScheduledEvent;

/// <summary>Command to edit an existing scheduled event in a Discord guild.</summary>
public record EditScheduledEventCommand(
    string? GuildId,
    string EventId,
    string? Name,
    string? Description,
    string? StartTime,
    string? EndTime) : IRequest<string>;
