namespace DiscordMcp.Features.Events.ListScheduledEvents;

[McpServerToolType]
public sealed class ListScheduledEventsTool(IMediator mediator)
{
    [McpServerTool(Name = "list_scheduled_events"), Description("List all scheduled events in a Discord guild")]
    public Task<string> ListScheduledEvents(
        [Description("Guild/server ID (uses DISCORD_GUILD_ID env var if omitted)")] string? guildId = null)
        => mediator.Send(new ListScheduledEventsQuery(guildId));
}
