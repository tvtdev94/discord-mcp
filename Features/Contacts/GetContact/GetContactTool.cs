namespace DiscordMcp.Features.Contacts.GetContact;

[McpServerToolType]
public sealed class GetContactTool(IMediator mediator)
{
    [McpServerTool(Name = "get_contact"), Description("Look up the Discord user ID saved for a contact name")]
    public Task<string> GetContact(
        [Description("Contact name to look up")] string name)
        => mediator.Send(new GetContactQuery(name));
}
