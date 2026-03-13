namespace DiscordMcp.Features.Categories.CreateCategory;

public sealed class CreateCategoryHandler(DiscordSocketClient client)
    : IRequestHandler<CreateCategoryCommand, string>
{
    public async Task<string> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) throw new ArgumentException("name cannot be null.");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        var category = await guild.CreateCategoryChannelAsync(request.Name);
        return $"Created new category: {category.Name} (ID: {category.Id})";
    }
}
