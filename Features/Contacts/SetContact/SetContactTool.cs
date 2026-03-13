namespace DiscordMcp.Features.Contacts.SetContact;

[McpServerToolType]
public sealed class SetContactTool(IMediator mediator)
{
    [McpServerTool(Name = "set_contact"), Description("Save a name → Discord user ID mapping for easy tagging")]
    public Task<string> SetContact(
        [Description("Friendly name for the contact (e.g. 'tuan tran')")] string name,
        [Description("Discord user ID (snowflake)")] string userId)
        => mediator.Send(new SetContactCommand(name, userId));
}
