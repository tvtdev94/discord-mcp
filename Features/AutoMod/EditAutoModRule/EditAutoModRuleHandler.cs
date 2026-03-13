using Discord;

namespace DiscordMcp.Features.AutoMod.EditAutoModRule;

public sealed class EditAutoModRuleHandler(DiscordSocketClient client)
    : IRequestHandler<EditAutoModRuleCommand, string>
{
    public async Task<string> Handle(EditAutoModRuleCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RuleId)) throw new ArgumentException("ruleId cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var rule = await guild.GetAutoModRuleAsync(SafeParser.ParseUlong(request.RuleId, "ruleId"))
            ?? throw new ArgumentException($"AutoMod rule '{request.RuleId}' not found.");

        await rule.ModifyAsync(props =>
        {
            if (request.Name is not null)    props.Name    = request.Name;
            if (request.Enabled is not null) props.Enabled = request.Enabled.Value;
        });

        return $"AutoMod rule updated: **{rule.Name}** (ID: {rule.Id})";
    }
}
