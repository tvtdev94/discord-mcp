namespace DiscordMcp.Features.Contacts.ListContacts;

[McpServerToolType]
public sealed class ListContactsTool(IMediator mediator)
{
    [McpServerTool(Name = "list_contacts"), Description("List all saved name → Discord user ID contact mappings")]
    public Task<string> ListContacts()
        => mediator.Send(new ListContactsQuery());
}
