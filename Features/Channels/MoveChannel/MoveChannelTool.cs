namespace DiscordMcp.Features.Channels.MoveChannel;

[McpServerToolType]
public sealed class MoveChannelTool(IMediator mediator)
{
    [McpServerTool(Name = "move_channel"), Description("Move a channel to a new position index within its category or server")]
    public Task<string> MoveChannel(
        [Description("Channel ID")] string channelId,
        [Description("New position index (0-based)")] int position)
        => mediator.Send(new MoveChannelCommand(channelId, position));
}
