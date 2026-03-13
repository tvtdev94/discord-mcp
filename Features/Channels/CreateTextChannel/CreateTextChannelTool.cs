namespace DiscordMcp.Features.Channels.CreateTextChannel;

[McpServerToolType]
public sealed class CreateTextChannelTool(IMediator mediator)
{
    [McpServerTool(Name = "create_text_channel"), Description("Create a new text channel")]
    public Task<string> CreateTextChannel(
        [Description("Discord server ID (uses default if omitted)")] string? guildId,
        [Description("Channel name")] string name,
        [Description("Category ID to place the channel in (optional)")] string? categoryId = null)
        => mediator.Send(new CreateTextChannelCommand(guildId, name, categoryId));
}
