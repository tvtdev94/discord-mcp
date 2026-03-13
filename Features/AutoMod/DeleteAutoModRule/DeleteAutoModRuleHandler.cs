using Discord;

namespace DiscordMcp.Features.AutoMod.DeleteAutoModRule;

public sealed class DeleteAutoModRuleHandler(DiscordSocketClient client)
    : IRequestHandler<DeleteAutoModRuleCommand, string>
{
    public async Task<string> Handle(DeleteAutoModRuleCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RuleId)) throw new ArgumentException("ruleId cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var rule = await guild.GetAutoModRuleAsync(SafeParser.ParseUlong(request.RuleId, "ruleId"))
            ?? throw new ArgumentException($"AutoMod rule '{request.RuleId}' not found.");

        var ruleName = rule.Name;
        await rule.DeleteAsync();

        return $"AutoMod rule deleted: **{ruleName}** (ID: {request.RuleId})";
    }
}
