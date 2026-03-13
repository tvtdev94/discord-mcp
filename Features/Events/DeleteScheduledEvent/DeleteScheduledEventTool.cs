namespace DiscordMcp.Features.Events.DeleteScheduledEvent;

[McpServerToolType]
public sealed class DeleteScheduledEventTool(IMediator mediator)
{
    [McpServerTool(Name = "delete_scheduled_event"), Description("Delete a scheduled event from a Discord guild")]
    public Task<string> DeleteScheduledEvent(
        [Description("ID of the scheduled event to delete")] string eventId,
        [Description("Guild/server ID (uses DISCORD_GUILD_ID env var if omitted)")] string? guildId = null)
        => mediator.Send(new DeleteScheduledEventCommand(guildId, eventId));
}
