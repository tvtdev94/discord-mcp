namespace DiscordMcp.Features.Categories.ListChannelsInCategory;

[McpServerToolType]
public sealed class ListChannelsInCategoryTool(IMediator mediator)
{
    [McpServerTool(Name = "list_channels_in_category"), Description("List all channels in a specific category")]
    public Task<string> ListChannelsInCategory(
        [Description("Discord server ID (uses default if omitted)")] string? guildId,
        [Description("Discord category ID")] string categoryId)
        => mediator.Send(new ListChannelsInCategoryQuery(guildId, categoryId));
}
