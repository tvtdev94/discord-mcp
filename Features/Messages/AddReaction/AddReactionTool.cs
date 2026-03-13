namespace DiscordMcp.Features.Messages.AddReaction;

[McpServerToolType]
public sealed class AddReactionTool(IMediator mediator)
{
    [McpServerTool(Name = "add_reaction"), Description("Add a reaction (emoji) to a specific message")]
    public Task<string> AddReaction(
        [Description("Discord channel ID")] string channelId,
        [Description("Discord message ID")] string messageId,
        [Description("Emoji (Unicode character, e.g. \u2764\ufe0f)")] string emoji)
        => mediator.Send(new AddReactionCommand(channelId, messageId, emoji));
}
