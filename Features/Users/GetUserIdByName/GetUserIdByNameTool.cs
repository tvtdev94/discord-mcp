namespace DiscordMcp.Features.Users.GetUserIdByName;

[McpServerToolType]
public sealed class GetUserIdByNameTool(IMediator mediator)
{
    [McpServerTool(Name = "get_user_id_by_name"), Description("Get a Discord user's ID by username in a guild for ping usage <@id>")]
    public Task<string> GetUserIdByName(
        [Description("Discord username (optionally username#discriminator)")] string username,
        [Description("Discord server ID (uses default if omitted)")] string? guildId = null)
        => mediator.Send(new GetUserIdByNameQuery(username, guildId));
}
