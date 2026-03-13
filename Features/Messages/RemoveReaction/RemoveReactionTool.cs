namespace DiscordMcp.Features.Messages.RemoveReaction;

[McpServerToolType]
public sealed class RemoveReactionTool(IMediator mediator)
{
    [McpServerTool(Name = "remove_reaction"), Description("Remove a specified reaction (emoji) from a message")]
    public Task<string> RemoveReaction(
        [Description("Discord channel ID")] string channelId,
        [Description("Discord message ID")] string messageId,
        [Description("Emoji (Unicode character, e.g. \u2764\ufe0f)")] string emoji)
        => mediator.Send(new RemoveReactionCommand(channelId, messageId, emoji));
}
