using Discord;

namespace DiscordMcp.Features.Contacts.TagContact;

public sealed class TagContactHandler(ContactStore store, DiscordSocketClient client, OperatorContext operatorContext)
    : IRequestHandler<TagContactQuery, string>
{
    public async Task<string> Handle(TagContactQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name cannot be empty.");
        if (string.IsNullOrWhiteSpace(request.ChannelId))
            throw new ArgumentException("ChannelId cannot be empty.");

        if (!store.TryGet(request.Name, out var userId))
            throw new ArgumentException($"Contact \"{request.Name.Trim()}\" not found.");

        var channel = client.GetChannel(SafeParser.ParseUlong(request.ChannelId, "channelId")) as Discord.IMessageChannel
            ?? throw new ArgumentException($"Channel '{request.ChannelId}' not found.");

        var raw = $"<@{userId}> {request.Message ?? ""}".TrimEnd();
        var content = await MessagePrefixHelper.PrependPrefixIfNeededAsync(channel, client, operatorContext, raw);
        var sent = await channel.SendMessageAsync(content);
        return $"Tagged {request.Name.Trim()} (<@{userId}>) in channel. Message link: {sent.GetJumpUrl()}";
    }
}
