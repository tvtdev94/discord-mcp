namespace DiscordMcp.Features.AutoMod.CreateAutoModRule;

/// <summary>Command to create a new AutoMod rule in a Discord server.</summary>
public record CreateAutoModRuleCommand(
    string? GuildId,
    string Name,
    string TriggerType,
    string? Keywords,
    bool Enabled) : IRequest<string>;
