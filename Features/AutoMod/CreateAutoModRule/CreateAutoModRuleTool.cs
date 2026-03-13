namespace DiscordMcp.Features.AutoMod.CreateAutoModRule;

[McpServerToolType]
public sealed class CreateAutoModRuleTool(IMediator mediator)
{
    [McpServerTool(Name = "create_automod_rule"), Description("Create a new AutoMod rule in a Discord server")]
    public Task<string> CreateAutoModRule(
        [Description("Rule name")] string name,
        [Description("Trigger type: keyword, spam, mention_spam, harmful_link")] string triggerType,
        [Description("Discord server ID")] string? guildId = null,
        [Description("Comma-separated keywords (required for 'keyword' trigger type)")] string? keywords = null,
        [Description("Whether the rule is enabled")] bool enabled = true)
        => mediator.Send(new CreateAutoModRuleCommand(guildId, name, triggerType, keywords, enabled));
}
