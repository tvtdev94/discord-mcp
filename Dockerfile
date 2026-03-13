# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY DiscordMcp.Server.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish DiscordMcp.Server.csproj -c Release -o /app --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine
WORKDIR /app
COPY --from=build /app .

# Non-root user for security
RUN adduser -D appuser
USER appuser

ENTRYPOINT ["dotnet", "DiscordMcp.Server.dll"]
