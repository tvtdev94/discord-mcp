namespace DiscordMcp.Features.Channels.EditChannel;

[McpServerToolType]
public sealed class EditChannelTool(IMediator mediator)
{
    [McpServerTool(Name = "edit_channel"), Description("Edit the name and/or topic of a text channel")]
    public Task<string> EditChannel(
        [Description("Channel ID")] string channelId,
        [Description("New channel name (optional)")] string? name = null,
        [Description("New channel topic (optional)")] string? topic = null)
        => mediator.Send(new EditChannelCommand(channelId, name, topic));
}
