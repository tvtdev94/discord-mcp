namespace DiscordMcp.Features.Events.CreateScheduledEvent;

/// <summary>Command to create a new scheduled event in a Discord guild.</summary>
public record CreateScheduledEventCommand(
    string? GuildId,
    string Name,
    string StartTime,
    string? EndTime,
    string? Description,
    string? ChannelId) : IRequest<string>;
