namespace DiscordMcp.Features.Moderation.PruneMembers;

[McpServerToolType]
public sealed class PruneMembersTool(IMediator mediator)
{
    [McpServerTool(Name = "prune_members"), Description("Prune inactive members from the Discord server")]
    public Task<string> PruneMembers(
        [Description("Discord server ID")] string? guildId,
        [Description("Number of days of inactivity (1-30)")] int days)
        => mediator.Send(new PruneMembersCommand(guildId, days));
}
