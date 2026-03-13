namespace DiscordMcp.Features.Invites.DeleteInvite;

/// <summary>Command to delete an invite by its code from a Discord guild.</summary>
public record DeleteInviteCommand(string? GuildId, string InviteCode) : IRequest<string>;
