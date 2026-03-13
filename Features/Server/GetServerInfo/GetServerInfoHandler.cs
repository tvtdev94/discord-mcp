namespace DiscordMcp.Features.Server.GetServerInfo;

/// <summary>Handles the GetServerInfoQuery — reads guild metadata from the socket cache.</summary>
public sealed class GetServerInfoHandler(DiscordSocketClient client)
    : IRequestHandler<GetServerInfoQuery, string>
{
    public Task<string> Handle(GetServerInfoQuery request, CancellationToken cancellationToken)
    {
        var guild = GuildResolver.Resolve(client, request.GuildId);

        var ownerName         = guild.Owner?.Username ?? "unknown";
        int textChannelCount  = guild.TextChannels.Count;
        int voiceChannelCount = guild.VoiceChannels.Count;
        int categoryCount     = guild.CategoryChannels.Count;
        string creationDate   = guild.CreatedAt.UtcDateTime.ToString("yyyy-MM-dd");
        int boostCount        = guild.PremiumSubscriptionCount;
        string boostTier      = guild.PremiumTier.ToString();

        var result = $"""
                Server Name: {guild.Name}
                Server ID: {guild.Id}
                Owner: {ownerName}
                Created On: {creationDate}
                Members: {guild.MemberCount}
                Channels:
                 - Text: {textChannelCount}
                 - Voice: {voiceChannelCount}
                 - Categories: {categoryCount}
                Boosts:
                 - Count: {boostCount}
                 - Tier: {boostTier}
                """;

        return Task.FromResult(result);
    }
}
