#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Team1FirstProject.csproj", "."]
RUN dotnet restore "./Team1FirstProject.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Team1FirstProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Team1FirstProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Team1FirstProject.dll"]