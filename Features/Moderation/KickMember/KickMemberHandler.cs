namespace DiscordMcp.Features.Moderation.KickMember;

public sealed class KickMemberHandler(DiscordSocketClient client)
    : IRequestHandler<KickMemberCommand, string>
{
    public async Task<string> Handle(KickMemberCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId)) throw new ArgumentException("userId cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var member = guild.GetUser(SafeParser.ParseUlong(request.UserId, "userId"))
            ?? throw new ArgumentException("Member not found in server by userId.");

        await member.KickAsync(request.Reason);
        return $"User {request.UserId} has been kicked successfully.";
    }
}
