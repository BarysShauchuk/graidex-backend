#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
# EXPOSE 80
# EXPOSE 7000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Graidex.API/Graidex.API.csproj", "Graidex.API/"]
COPY ["Graidex.Application/Graidex.Application.csproj", "Graidex.Application/"]
COPY ["Graidex.Domain/Graidex.Domain.csproj", "Graidex.Domain/"]
COPY ["Graidex.Infrastructure/Graidex.Infrastructure.csproj", "Graidex.Infrastructure/"]
RUN dotnet restore "Graidex.API/Graidex.API.csproj"
COPY . .
WORKDIR "/src/Graidex.API"
RUN dotnet build "Graidex.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Graidex.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Graidex.API.dll"]