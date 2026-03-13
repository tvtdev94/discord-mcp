namespace DiscordMcp.Features.AutoMod.ListAutoModRules;

[McpServerToolType]
public sealed class ListAutoModRulesTool(IMediator mediator)
{
    [McpServerTool(Name = "list_automod_rules"), Description("List all AutoMod rules in a Discord server")]
    public Task<string> ListAutoModRules(
        [Description("Discord server ID")] string? guildId = null)
        => mediator.Send(new ListAutoModRulesQuery(guildId));
}
