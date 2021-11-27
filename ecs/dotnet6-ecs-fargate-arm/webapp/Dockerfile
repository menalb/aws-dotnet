FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine-arm64v8 as base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine-arm64v8 AS build
WORKDIR /src
COPY ["webapp.csproj", ""]
RUN dotnet restore "webapp.csproj" -r linux-musl-arm64

COPY . .
WORKDIR "/src/"
RUN dotnet build "webapp.csproj" -c Release -o /app/build -r linux-musl-arm64 --self-contained false --no-restore

FROM build AS publish
RUN dotnet publish "webapp.csproj" -c Release -o /app/publish -r linux-musl-arm64 --self-contained false --no-restore

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "webapp.dll"]
