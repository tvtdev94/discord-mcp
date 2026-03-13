namespace DiscordMcp.Features.Moderation.UnbanMember;

public sealed class UnbanMemberHandler(DiscordSocketClient client)
    : IRequestHandler<UnbanMemberCommand, string>
{
    public async Task<string> Handle(UnbanMemberCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId)) throw new ArgumentException("userId cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        await guild.RemoveBanAsync(SafeParser.ParseUlong(request.UserId, "userId"));
        return $"User {request.UserId} has been unbanned successfully.";
    }
}
