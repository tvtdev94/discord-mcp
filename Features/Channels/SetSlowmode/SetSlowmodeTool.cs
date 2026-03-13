namespace DiscordMcp.Features.Channels.SetSlowmode;

[McpServerToolType]
public sealed class SetSlowmodeTool(IMediator mediator)
{
    [McpServerTool(Name = "set_slowmode"), Description("Set the slowmode interval for a text channel (0 to disable, max 21600 seconds)")]
    public Task<string> SetSlowmode(
        [Description("Text channel ID")] string channelId,
        [Description("Slowmode interval in seconds (0 to disable, max 21600)")] int seconds)
        => mediator.Send(new SetSlowmodeCommand(channelId, seconds));
}
