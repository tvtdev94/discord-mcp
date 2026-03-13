namespace DiscordMcp.Features.AutoMod.EditAutoModRule;

[McpServerToolType]
public sealed class EditAutoModRuleTool(IMediator mediator)
{
    [McpServerTool(Name = "edit_automod_rule"), Description("Edit an existing AutoMod rule")]
    public Task<string> EditAutoModRule(
        [Description("AutoMod rule ID")] string ruleId,
        [Description("Discord server ID")] string? guildId = null,
        [Description("New rule name")] string? name = null,
        [Description("Enable or disable the rule")] bool? enabled = null)
        => mediator.Send(new EditAutoModRuleCommand(guildId, ruleId, name, enabled));
}
