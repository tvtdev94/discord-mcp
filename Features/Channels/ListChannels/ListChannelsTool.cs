namespace DiscordMcp.Features.Channels.ListChannels;

[McpServerToolType]
public sealed class ListChannelsTool(IMediator mediator)
{
    [McpServerTool(Name = "list_channels"), Description("List all channels in a Discord server")]
    public Task<string> ListChannels(
        [Description("Discord server ID (uses default if omitted)")] string? guildId = null)
        => mediator.Send(new ListChannelsQuery(guildId));
}
