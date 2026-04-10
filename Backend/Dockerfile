# syntax=docker/dockerfile:1.7

FROM mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim AS build
WORKDIR /src

# Copy project files first for better restore cache.
COPY Backend/Directory.Build.props Backend/
COPY Backend/src/TarotNow.Domain/TarotNow.Domain.csproj Backend/src/TarotNow.Domain/
COPY Backend/src/TarotNow.Application/TarotNow.Application.csproj Backend/src/TarotNow.Application/
COPY Backend/src/TarotNow.Infrastructure/TarotNow.Infrastructure.csproj Backend/src/TarotNow.Infrastructure/
COPY Backend/src/TarotNow.Api/TarotNow.Api.csproj Backend/src/TarotNow.Api/
RUN dotnet restore Backend/src/TarotNow.Api/TarotNow.Api.csproj

COPY Backend/ Backend/
RUN dotnet publish Backend/src/TarotNow.Api/TarotNow.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0-bookworm-slim AS runtime
RUN apt-get update \
    && apt-get upgrade -y \
    && apt-get install -y --no-install-recommends ffmpeg ca-certificates wget \
    && rm -rf /var/lib/apt/lists/*

RUN if ! getent group app > /dev/null; then groupadd --gid 10001 app; fi \
    && if ! id -u app > /dev/null 2>&1; then useradd --uid 10001 --gid app --create-home --home-dir /home/app --shell /usr/sbin/nologin app; fi

WORKDIR /app
COPY --from=build /app/publish ./

RUN mkdir -p /app/wwwroot/uploads \
    && chown -R app:app /app

USER app

ENV ASPNETCORE_URLS=http://0.0.0.0:5037
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 5037

ENTRYPOINT ["dotnet", "TarotNow.Api.dll"]
