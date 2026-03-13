<!-- mcp-name: discord-mcp-server -->

# Discord MCP Server

A [Model Context Protocol (MCP)](https://modelcontextprotocol.io) server that exposes Discord bot operations as MCP tools. Built with C#/.NET 9, Discord.Net, and MediatR.

Allows AI assistants (Claude, etc.) to manage Discord servers — send messages, manage channels, roles, moderation, webhooks, and more — via the MCP protocol.

## Features

70+ MCP tools organized by domain:

| Domain | Tools |
|--------|-------|
| Messages | send, read, edit, delete, pin/unpin, reactions |
| Channels | create text/voice, edit, delete, move, permissions, slowmode |
| Roles | create, edit, delete, assign, remove, list |
| Moderation | ban, kick, timeout, prune, bulk delete, audit log |
| Categories | create, delete, find, list channels |
| Threads | create, archive, unarchive, list active/archived |
| Webhooks | create, delete, list, send messages |
| Events | create, edit, delete, list scheduled events |
| Emojis | create, delete, list |
| AutoMod | create, edit, delete, list rules |
| Members | get info, list, set nickname |
| Invites | create, delete, list |
| Server | get info, edit |
| Users | DMs (send, read, edit, delete), find by name |
| Contacts | name→userId mapping store (set, get, list, remove, tag) |

## Prerequisites

- [Docker](https://docs.docker.com/get-docker/)
- A [Discord bot token](https://discord.com/developers/applications) with appropriate permissions

### Required Bot Permissions

- Send Messages, Read Message History, Manage Messages
- Manage Channels, Manage Roles, Manage Guild
- Kick Members, Ban Members, Moderate Members
- Manage Webhooks, Manage Emojis, Manage Events
- Read Members (privileged intent), Message Content (privileged intent)

## Setup

### Step 1: Get your IDs

Enable **Developer Mode** in Discord (Settings → Advanced → Developer Mode), then:
- **Bot token** — from [Discord Developer Portal](https://discord.com/developers/applications) → Bot → Token
- **Server ID** — Right-click server name → "Copy Server ID"
- **Your user ID** — Right-click your name → "Copy User ID"

### Step 2: Configure your MCP client

Replace the 3 values with your own IDs.

```json
{
  "mcpServers": {
    "discord": {
      "command": "docker",
      "args": ["run", "-i", "--rm",
        "-e", "DISCORD_TOKEN",
        "-e", "DISCORD_GUILD_ID",
        "-e", "DISCORD_OPERATOR_ID",
        "ghcr.io/tvtdev94/discord-mcp:latest"],
      "env": {
        "DISCORD_TOKEN": "your-bot-token",
        "DISCORD_GUILD_ID": "your-server-id",
        "DISCORD_OPERATOR_ID": "your-user-id"
      }
    }
  }
}
```

### Claude Code

```bash
claude mcp add discord -- docker run -i --rm \
  -e DISCORD_TOKEN="your-bot-token" \
  -e DISCORD_GUILD_ID="your-server-id" \
  -e DISCORD_OPERATOR_ID="your-user-id" \
  ghcr.io/tvtdev94/discord-mcp:latest
```

### Step 3: Verify

Ask Claude: *"List my Discord channels"* — if it returns channel names, you're set.

## Environment Variables

| Variable | Required | Description |
|----------|----------|-------------|
| `DISCORD_TOKEN` | Yes | Discord bot token |
| `DISCORD_GUILD_ID` | No | Default server ID (most tools fall back to this) |
| `DISCORD_OPERATOR_ID` | No | Your Discord user ID — enables message attribution and audit logging |
| `DISCORD_CONTACTS_PATH` | No | Custom path for contacts.json |
| `DISCORD_AUDIT_PATH` | No | Custom path for audit logs (default: app directory) |

## Developer Setup

```bash
git clone https://github.com/tvtdev94/discord-mcp.git
cd discord-mcp
dotnet build
```

Build & run with Docker:

```bash
docker build -t discord-mcp .
docker run -i --rm \
  -e DISCORD_TOKEN="your-token" \
  -e DISCORD_GUILD_ID="your-server-id" \
  -e DISCORD_OPERATOR_ID="your-user-id" \
  discord-mcp
```

## Architecture

```
├── Program.cs                       # Host setup, DI, MCP server config
├── GlobalUsings.cs                  # Shared using statements
├── Infrastructure/
│   ├── DiscordClientHostedService   # Discord client lifecycle
│   ├── GuildResolver                # Guild ID resolution with fallback
│   ├── ContactStore                 # JSON-backed name→userId store
│   ├── OperatorContext              # Operator identity (DISCORD_OPERATOR_ID)
│   ├── AuditLogger                  # JSONL audit trail per operator/day
│   ├── AuditLoggingBehavior         # MediatR pipeline — auto-logs all actions
│   ├── MessagePrefixHelper          # Smart "[via @User]" prefix deduplication
│   ├── MessageFormatter             # Shared message formatting
│   └── SafeParser                   # Input parsing with descriptive errors
└── Features/                        # Vertical slice architecture
    └── {Domain}/
        └── {Action}/
            ├── *Tool.cs             # MCP tool definition
            ├── *Command.cs          # MediatR command/query record
            └── *Handler.cs          # Business logic
```

Each feature follows the **vertical slice** pattern with MediatR CQRS:
- **Tool** — MCP tool endpoint, delegates to MediatR
- **Command/Query** — immutable request record
- **Handler** — business logic, Discord API calls

## License

MIT
