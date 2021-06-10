#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["GossipCheck.API/GossipCheck.API.csproj", "GossipCheck.API/"]
COPY ["GossipCheck.DAO/GossipCheck.DAO.csproj", "GossipCheck.DAO/"]
COPY ["GossipCheck.BLL/GossipCheck.BLL.csproj", "GossipCheck.BLL/"]
RUN dotnet restore "GossipCheck.API/GossipCheck.API.csproj" --disable-parallel
COPY . .
WORKDIR "/src/GossipCheck.API"
RUN dotnet build "GossipCheck.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GossipCheck.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GossipCheck.API.dll"]