namespace DiscordMcp.Features.Channels.FindChannel;

[McpServerToolType]
public sealed class FindChannelTool(IMediator mediator)
{
    [McpServerTool(Name = "find_channel"), Description("Find a channel type and ID using name and server ID")]
    public Task<string> FindChannel(
        [Description("Discord server ID (uses default if omitted)")] string? guildId,
        [Description("Channel name to search for")] string channelName)
        => mediator.Send(new FindChannelQuery(guildId, channelName));
}
