namespace DiscordMcp.Features.Categories.DeleteCategory;

[McpServerToolType]
public sealed class DeleteCategoryTool(IMediator mediator)
{
    [McpServerTool(Name = "delete_category"), Description("Delete a category")]
    public Task<string> DeleteCategory(
        [Description("Discord server ID (uses default if omitted)")] string? guildId,
        [Description("Discord category ID")] string categoryId)
        => mediator.Send(new DeleteCategoryCommand(guildId, categoryId));
}
