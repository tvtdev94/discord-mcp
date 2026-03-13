namespace DiscordMcp.Features.AutoMod.DeleteAutoModRule;

[McpServerToolType]
public sealed class DeleteAutoModRuleTool(IMediator mediator)
{
    [McpServerTool(Name = "delete_automod_rule"), Description("Delete an AutoMod rule from a Discord server")]
    public Task<string> DeleteAutoModRule(
        [Description("AutoMod rule ID")] string ruleId,
        [Description("Discord server ID")] string? guildId = null)
        => mediator.Send(new DeleteAutoModRuleCommand(guildId, ruleId));
}
