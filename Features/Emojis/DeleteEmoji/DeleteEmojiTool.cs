namespace DiscordMcp.Features.Emojis.DeleteEmoji;

[McpServerToolType]
public sealed class DeleteEmojiTool(IMediator mediator)
{
    [McpServerTool(Name = "delete_emoji"), Description("Delete a custom emoji from a Discord server")]
    public Task<string> DeleteEmoji(
        [Description("Emoji ID to delete")] string emojiId,
        [Description("Discord server ID")] string? guildId = null)
        => mediator.Send(new DeleteEmojiCommand(guildId, emojiId));
}
