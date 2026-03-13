using Discord;

namespace DiscordMcp.Features.AutoMod.ListAutoModRules;

public sealed class ListAutoModRulesHandler(DiscordSocketClient client)
    : IRequestHandler<ListAutoModRulesQuery, string>
{
    public async Task<string> Handle(ListAutoModRulesQuery request, CancellationToken cancellationToken)
    {
        var guild = GuildResolver.Resolve(client, request.GuildId);

        var rules = await guild.GetAutoModRulesAsync();
        if (rules.Length == 0) return "No AutoMod rules found.";

        var lines = rules.Select(r =>
            $"• **{r.Name}** (ID: {r.Id})\n" +
            $"  Trigger: {r.TriggerType} | Enabled: {r.Enabled}\n" +
            $"  Actions: {string.Join(", ", r.Actions.Select(a => a.Type.ToString()))}");

        return $"AutoMod Rules ({rules.Length}):\n{string.Join("\n", lines)}";
    }
}
