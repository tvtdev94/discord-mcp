namespace DiscordMcp.Features.Channels.CreateTextChannel;

public sealed class CreateTextChannelHandler(DiscordSocketClient client)
    : IRequestHandler<CreateTextChannelCommand, string>
{
    public async Task<string> Handle(CreateTextChannelCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) throw new ArgumentException("name cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        if (!string.IsNullOrWhiteSpace(request.CategoryId))
        {
            var category = guild.GetCategoryChannel(SafeParser.ParseUlong(request.CategoryId, "categoryId"))
                ?? throw new ArgumentException("Category not found by categoryId.");

            var textChannel = await guild.CreateTextChannelAsync(request.Name, props =>
                props.CategoryId = category.Id);

            return $"Created new text channel: {textChannel.Name} (ID: {textChannel.Id}) in category: {category.Name}";
        }

        var channel = await guild.CreateTextChannelAsync(request.Name);
        return $"Created new text channel: {channel.Name} (ID: {channel.Id})";
    }
}
