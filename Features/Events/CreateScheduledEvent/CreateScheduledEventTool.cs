namespace DiscordMcp.Features.Events.CreateScheduledEvent;

[McpServerToolType]
public sealed class CreateScheduledEventTool(IMediator mediator)
{
    [McpServerTool(Name = "create_scheduled_event"), Description("Create a new scheduled event in a Discord guild")]
    public Task<string> CreateScheduledEvent(
        [Description("Name of the event")] string name,
        [Description("Event start time in ISO 8601 format (e.g. 2025-06-01T18:00:00+00:00)")] string startTime,
        [Description("Guild/server ID (uses DISCORD_GUILD_ID env var if omitted)")] string? guildId = null,
        [Description("Event end time in ISO 8601 format. Required for external events (no channelId).")] string? endTime = null,
        [Description("Optional description for the event")] string? description = null,
        [Description("Voice channel ID to host the event in. If omitted, creates an external event.")] string? channelId = null)
        => mediator.Send(new CreateScheduledEventCommand(guildId, name, startTime, endTime, description, channelId));
}
