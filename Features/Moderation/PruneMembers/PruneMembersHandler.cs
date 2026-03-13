namespace DiscordMcp.Features.Moderation.PruneMembers;

public sealed class PruneMembersHandler(DiscordSocketClient client)
    : IRequestHandler<PruneMembersCommand, string>
{
    public async Task<string> Handle(PruneMembersCommand request, CancellationToken cancellationToken)
    {
        if (request.Days < 1 || request.Days > 30)
            throw new ArgumentException("days must be between 1 and 30.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var pruned = await guild.PruneUsersAsync(request.Days);
        return $"Pruned {pruned} inactive member(s) who had not been active in the last {request.Days} day(s).";
    }
}
