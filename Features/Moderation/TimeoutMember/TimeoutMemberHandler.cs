using Discord;

namespace DiscordMcp.Features.Moderation.TimeoutMember;

public sealed class TimeoutMemberHandler(DiscordSocketClient client)
    : IRequestHandler<TimeoutMemberCommand, string>
{
    public async Task<string> Handle(TimeoutMemberCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId)) throw new ArgumentException("userId cannot be null.");
        if (request.DurationMinutes <= 0) throw new ArgumentException("durationMinutes must be greater than 0.");
        if (request.DurationMinutes > 40320) throw new ArgumentException("durationMinutes cannot exceed 40320 (28 days).");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var member = guild.GetUser(SafeParser.ParseUlong(request.UserId, "userId"))
            ?? throw new ArgumentException("Member not found in server by userId.");

        var options = new RequestOptions { AuditLogReason = request.Reason };
        await member.SetTimeOutAsync(TimeSpan.FromMinutes(request.DurationMinutes), options);

        return $"User {request.UserId} has been timed out for {request.DurationMinutes} minute(s).";
    }
}
