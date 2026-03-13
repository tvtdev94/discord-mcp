namespace DiscordMcp.Features.AutoMod.ListAutoModRules;

/// <summary>Query to list all AutoMod rules in a Discord server.</summary>
public record ListAutoModRulesQuery(string? GuildId) : IRequest<string>;
