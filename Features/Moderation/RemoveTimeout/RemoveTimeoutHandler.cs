namespace DiscordMcp.Features.Moderation.RemoveTimeout;

public sealed class RemoveTimeoutHandler(DiscordSocketClient client)
    : IRequestHandler<RemoveTimeoutCommand, string>
{
    public async Task<string> Handle(RemoveTimeoutCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId)) throw new ArgumentException("userId cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var member = guild.GetUser(SafeParser.ParseUlong(request.UserId, "userId"))
            ?? throw new ArgumentException("Member not found in server by userId.");

        await member.RemoveTimeOutAsync();
        return $"Timeout removed from user {request.UserId} successfully.";
    }
}
