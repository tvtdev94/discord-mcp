namespace DiscordMcp.Features.Events.EditScheduledEvent;

[McpServerToolType]
public sealed class EditScheduledEventTool(IMediator mediator)
{
    [McpServerTool(Name = "edit_scheduled_event"), Description("Edit an existing scheduled event in a Discord guild")]
    public Task<string> EditScheduledEvent(
        [Description("ID of the scheduled event to edit")] string eventId,
        [Description("Guild/server ID (uses DISCORD_GUILD_ID env var if omitted)")] string? guildId = null,
        [Description("New name for the event")] string? name = null,
        [Description("New description for the event")] string? description = null,
        [Description("New start time in ISO 8601 format")] string? startTime = null,
        [Description("New end time in ISO 8601 format")] string? endTime = null)
        => mediator.Send(new EditScheduledEventCommand(guildId, eventId, name, description, startTime, endTime));
}
