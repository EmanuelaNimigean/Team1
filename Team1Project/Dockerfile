#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./Team1Project.csproj", "Team1Project/"]
RUN dotnet restore "Team1Project/Team1Project.csproj"
COPY . ./Team1Project
WORKDIR "/src/Team1Project"
RUN dotnet build "Team1Project.csproj" -c Release -o /app/build /p:AssemblyVersion=1.3

FROM build AS publish
RUN dotnet publish  "Team1Project.csproj" -c Release -o /app/publish /p:AssemblyVersion=1.3

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Team1Project.dll"]