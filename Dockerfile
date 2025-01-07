#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

ENV DOTNET_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["App/App.csproj", "App/"]
COPY ["App.Core/App.Core.csproj", "App.Core/"]
COPY ["App.Data/App.Data.csproj", "App.Data/"]
COPY ["App.Models/App.Models.csproj", "App.Models/"]
COPY ["App.Dtos/App.Dtos.csproj", "App.Dtos/"]
COPY ["App.Utitlities/App.Utitlities.csproj", "App.Utitlities/"]
RUN dotnet restore "./App/./App.csproj"
COPY . .
WORKDIR "/src/App"
RUN dotnet build "./App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.dll"]