namespace DiscordMcp.Features.Categories.DeleteCategory;

public sealed class DeleteCategoryHandler(DiscordSocketClient client)
    : IRequestHandler<DeleteCategoryCommand, string>
{
    public async Task<string> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var guild = GuildResolver.Resolve(client, request.GuildId);

        var category = guild.GetCategoryChannel(SafeParser.ParseUlong(request.CategoryId, "categoryId"))
            ?? throw new ArgumentException("Category not found by categoryId.");

        await category.DeleteAsync();
        return $"Deleted category: {category.Name}";
    }
}
