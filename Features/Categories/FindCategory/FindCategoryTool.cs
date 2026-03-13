namespace DiscordMcp.Features.Categories.FindCategory;

[McpServerToolType]
public sealed class FindCategoryTool(IMediator mediator)
{
    [McpServerTool(Name = "find_category"), Description("Find a category ID using name and server ID")]
    public Task<string> FindCategory(
        [Description("Discord server ID (uses default if omitted)")] string? guildId,
        [Description("Category name to search for")] string categoryName)
        => mediator.Send(new FindCategoryQuery(guildId, categoryName));
}
