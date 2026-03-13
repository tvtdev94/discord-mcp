namespace DiscordMcp.Features.Invites.CreateInvite;

/// <summary>Command to create an invite link for a Discord text channel.</summary>
public record CreateInviteCommand(string ChannelId, int? MaxAge, int? MaxUses) : IRequest<string>;
