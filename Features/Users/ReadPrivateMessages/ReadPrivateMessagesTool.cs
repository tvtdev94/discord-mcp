namespace DiscordMcp.Features.Users.ReadPrivateMessages;

[McpServerToolType]
public sealed class ReadPrivateMessagesTool(IMediator mediator)
{
    [McpServerTool(Name = "read_private_messages"), Description("Read recent message history from a specific user's DM")]
    public Task<string> ReadPrivateMessages(
        [Description("Discord user ID")] string userId,
        [Description("Number of messages to retrieve (default 100)")] string? count = null)
        => mediator.Send(new ReadPrivateMessagesQuery(userId, count));
}
