namespace DiscordMcp.Features.Channels.DeleteChannel;

[McpServerToolType]
public sealed class DeleteChannelTool(IMediator mediator)
{
    [McpServerTool(Name = "delete_channel"), Description("Delete a channel")]
    public Task<string> DeleteChannel(
        [Description("Discord server ID (uses default if omitted)")] string? guildId,
        [Description("Discord channel ID")] string channelId)
        => mediator.Send(new DeleteChannelCommand(guildId, channelId));
}
