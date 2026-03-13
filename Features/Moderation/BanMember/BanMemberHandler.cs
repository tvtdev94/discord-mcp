using Discord;

namespace DiscordMcp.Features.Moderation.BanMember;

public sealed class BanMemberHandler(DiscordSocketClient client)
    : IRequestHandler<BanMemberCommand, string>
{
    public async Task<string> Handle(BanMemberCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId)) throw new ArgumentException("userId cannot be null.");
        if (request.DeleteMessageDays < 0 || request.DeleteMessageDays > 7)
            throw new ArgumentException("deleteMessageDays must be between 0 and 7.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var userId = SafeParser.ParseUlong(request.UserId, "userId");
        var requestOptions = new RequestOptions { AuditLogReason = request.Reason };

        await guild.AddBanAsync(userId, request.DeleteMessageDays, request.Reason, requestOptions);
        return $"User {request.UserId} has been banned successfully.";
    }
}
