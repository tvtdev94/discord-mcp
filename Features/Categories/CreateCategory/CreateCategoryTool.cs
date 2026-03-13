namespace DiscordMcp.Features.Categories.CreateCategory;

[McpServerToolType]
public sealed class CreateCategoryTool(IMediator mediator)
{
    [McpServerTool(Name = "create_category"), Description("Create a new category for channels")]
    public Task<string> CreateCategory(
        [Description("Discord server ID (uses default if omitted)")] string? guildId,
        [Description("Category name")] string name)
        => mediator.Send(new CreateCategoryCommand(guildId, name));
}
