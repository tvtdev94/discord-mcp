namespace DiscordMcp.Features.AutoMod.DeleteAutoModRule;

/// <summary>Command to delete an AutoMod rule from a Discord server.</summary>
public record DeleteAutoModRuleCommand(string? GuildId, string RuleId) : IRequest<string>;
