namespace DiscordMcp.Features.Channels.CreateVoiceChannel;

public sealed class CreateVoiceChannelHandler(DiscordSocketClient client)
    : IRequestHandler<CreateVoiceChannelCommand, string>
{
    public async Task<string> Handle(CreateVoiceChannelCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) throw new ArgumentException("name cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        if (!string.IsNullOrWhiteSpace(request.CategoryId))
        {
            var category = guild.GetCategoryChannel(SafeParser.ParseUlong(request.CategoryId, "categoryId"))
                ?? throw new ArgumentException("Category not found by categoryId.");

            var voiceChannel = await guild.CreateVoiceChannelAsync(request.Name, props =>
                props.CategoryId = category.Id);

            return $"Created voice channel: {voiceChannel.Name} (ID: {voiceChannel.Id}) in category: {category.Name}.";
        }

        var channel = await guild.CreateVoiceChannelAsync(request.Name);
        return $"Created voice channel: {channel.Name} (ID: {channel.Id}).";
    }
}
