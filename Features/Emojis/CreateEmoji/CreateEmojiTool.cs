namespace DiscordMcp.Features.Emojis.CreateEmoji;

[McpServerToolType]
public sealed class CreateEmojiTool(IMediator mediator)
{
    [McpServerTool(Name = "create_emoji"), Description("Create a custom emoji in a Discord server from an image URL")]
    public Task<string> CreateEmoji(
        [Description("Emoji name (no spaces, alphanumeric + underscores)")] string name,
        [Description("URL of the image to use for the emoji")] string imageUrl,
        [Description("Discord server ID")] string? guildId = null)
        => mediator.Send(new CreateEmojiCommand(guildId, name, imageUrl));
}
