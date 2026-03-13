# Brainstorm: Attribution & Audit System

## Problem
Multiple users share same MCP server → all actions sent under bot name. No way to know who triggered what. Need:
1. Visual attribution on Discord (prefix)
2. Internal audit trail (JSON log)
3. Contact system already works, just need to leverage it for identity

## Agreed Solution

### 1. Operator Identity
- Env var `DISCORD_OPERATOR_ID` = Discord user ID of the person running this MCP instance
- On startup, resolve to username via Discord API
- Store as singleton `OperatorContext` { Id, Username, DisplayName }
- Each MCP instance = 1 operator (1 process per person)

### 2. Smart Message Prefix
- Format: `**[via @Username]** actual message`
- Only on FIRST message in consecutive sequence from same operator in same channel
- Logic: before sending, read last message in channel → if author == bot AND content starts with same operator prefix → skip prefix
- If different operator or non-bot message in between → add prefix
- Applies to: `send_message`, `tag_contact`, any tool that sends visible messages
- Does NOT apply to: moderation actions, role changes, channel edits (invisible)

### 3. Audit Log (JSON files)
- Path: `{DISCORD_AUDIT_PATH || AppBaseDir}/audit/{operatorId}/{yyyy-MM-dd}.json`
- Each entry: `{ timestamp, action, params, result, channelId?, targetId? }`
- Append-only JSONL (one JSON object per line, easy to parse)
- MediatR pipeline behavior wraps all handlers automatically
- No manual logging in each handler

## Architecture

```
Infrastructure/
├── OperatorContext.cs          # Singleton: resolves DISCORD_OPERATOR_ID → username
├── AuditLogger.cs              # JSONL file writer, folder per operator, file per day
├── AuditLoggingBehavior.cs     # MediatR IPipelineBehavior<,> — auto-logs all commands/queries
└── MessagePrefixHelper.cs      # Smart prefix: check last msg, decide prefix or not
```

### Key Design Decisions
- **MediatR pipeline behavior** for audit = zero changes to existing 80+ handlers. DRY.
- **JSONL** (not JSON array) = append-safe, no corruption on crash, easy `tail -f`
- **Prefix check via last message** = no in-memory state needed, stateless, works after restart
- **Operator per process** = simple, no auth needed, env var is enough

## Risks
- Bot rate limit if checking last message before every send (mitigate: cache per channel, 5s TTL)
- JSONL files grow over time (mitigate: document rotation, or add max-days env var later)
- Operator could set wrong ID (mitigate: validate on startup, log warning if user not found)

## Not Doing (YAGNI)
- Multi-operator per process (over-engineering)
- Database for audit (overkill for MCP server)
- Prefix on every single message (user explicitly rejected)
- Web UI for audit viewing (can use `jq` on JSONL files)
