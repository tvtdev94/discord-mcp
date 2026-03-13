using Discord;

namespace DiscordMcp.Features.AutoMod.CreateAutoModRule;

public sealed class CreateAutoModRuleHandler(DiscordSocketClient client)
    : IRequestHandler<CreateAutoModRuleCommand, string>
{
    public async Task<string> Handle(CreateAutoModRuleCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) throw new ArgumentException("name cannot be null.");

        var triggerType = request.TriggerType.ToLowerInvariant() switch
        {
            "keyword"      => AutoModTriggerType.Keyword,
            "spam"         => AutoModTriggerType.Spam,
            "mention_spam" => AutoModTriggerType.MentionSpam,
            "harmful_link" => AutoModTriggerType.HarmfulLink,
            _ => throw new ArgumentException($"Invalid triggerType '{request.TriggerType}'. Valid: keyword, spam, mention_spam, harmful_link.")
        };

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var rule = await guild.CreateAutoModRuleAsync(props =>
        {
            props.Name        = request.Name;
            props.TriggerType = triggerType;
            props.Enabled     = request.Enabled;
            props.Actions     = new AutoModRuleActionProperties[] { new() { Type = AutoModActionType.BlockMessage } };

            // Keyword filter only applies to the Keyword trigger type
            if (triggerType == AutoModTriggerType.Keyword && !string.IsNullOrWhiteSpace(request.Keywords))
            {
                var keywords = request.Keywords
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                props.KeywordFilter = keywords;
            }
        });

        return $"AutoMod rule created: **{rule.Name}** (ID: {rule.Id})\n" +
               $"Trigger: {rule.TriggerType} | Enabled: {rule.Enabled}";
    }
}
