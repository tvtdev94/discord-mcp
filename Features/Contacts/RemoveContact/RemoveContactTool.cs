namespace DiscordMcp.Features.Contacts.RemoveContact;

[McpServerToolType]
public sealed class RemoveContactTool(IMediator mediator)
{
    [McpServerTool(Name = "remove_contact"), Description("Remove a saved contact mapping by name")]
    public Task<string> RemoveContact(
        [Description("Contact name to remove")] string name)
        => mediator.Send(new RemoveContactCommand(name));
}
