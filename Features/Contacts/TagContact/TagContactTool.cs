namespace DiscordMcp.Features.Contacts.TagContact;

[McpServerToolType]
public sealed class TagContactTool(IMediator mediator)
{
    [McpServerTool(Name = "tag_contact"), Description("Tag a saved contact by name in a channel")]
    public Task<string> TagContact(
        [Description("Contact name (must be saved via set_contact)")] string name,
        [Description("Discord channel ID to send the tag in")] string channelId,
        [Description("Optional message to include alongside the mention")] string? message = null)
        => mediator.Send(new TagContactQuery(name, channelId, message));
}
