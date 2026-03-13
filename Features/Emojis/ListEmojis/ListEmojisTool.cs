namespace DiscordMcp.Features.Emojis.ListEmojis;

[McpServerToolType]
public sealed class ListEmojisTool(IMediator mediator)
{
    [McpServerTool(Name = "list_emojis"), Description("List all custom emojis in a Discord server")]
    public Task<string> ListEmojis(
        [Description("Discord server ID")] string? guildId = null)
        => mediator.Send(new ListEmojisQuery(guildId));
}
