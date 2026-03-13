namespace DiscordMcp.Features.AutoMod.EditAutoModRule;

/// <summary>Command to edit an existing AutoMod rule.</summary>
public record EditAutoModRuleCommand(
    string? GuildId,
    string RuleId,
    string? Name,
    bool? Enabled) : IRequest<string>;
