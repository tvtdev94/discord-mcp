namespace DiscordMcp.Features.Channels.CreateVoiceChannel;

[McpServerToolType]
public sealed class CreateVoiceChannelTool(IMediator mediator)
{
    [McpServerTool(Name = "create_voice_channel"), Description("Create a new voice channel in a Discord server")]
    public Task<string> CreateVoiceChannel(
        [Description("Discord server ID (uses default if omitted)")] string? guildId,
        [Description("Voice channel name")] string name,
        [Description("Category ID to place the channel in (optional)")] string? categoryId = null)
        => mediator.Send(new CreateVoiceChannelCommand(guildId, name, categoryId));
}
