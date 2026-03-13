namespace DiscordMcp.Features.Messages.AddMultipleReactions;

[McpServerToolType]
public sealed class AddMultipleReactionsTool(IMediator mediator)
{
    [McpServerTool(Name = "add_multiple_reactions"), Description("Add multiple reactions (emojis) to a specific message")]
    public Task<string> AddMultipleReactions(
        [Description("Discord channel ID")] string channelId,
        [Description("Discord message ID")] string messageId,
        [Description("Comma-separated list of emoji Unicode characters, e.g. \"\u2764\ufe0f,\ud83d\udc4d,\ud83d\ude80\"")] string emojis)
        => mediator.Send(new AddMultipleReactionsCommand(channelId, messageId, emojis));
}
