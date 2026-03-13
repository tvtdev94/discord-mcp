namespace DiscordMcp.Features.Server.EditServer;

public sealed class EditServerHandler(DiscordSocketClient client)
    : IRequestHandler<EditServerCommand, string>
{
    public async Task<string> Handle(EditServerCommand request, CancellationToken cancellationToken)
    {
        if (request.Name is null)
            throw new ArgumentException("name must be provided (description is not modifiable via the bot API).");

        var guild = GuildResolver.Resolve(client, request.GuildId);

        // GuildProperties does not expose Description — only Name is editable via ModifyAsync
        await guild.ModifyAsync(x =>
        {
            if (request.Name is not null) x.Name = request.Name;
        });

        return $"Server updated: {guild.Name} (ID: {guild.Id}).";
    }
}
