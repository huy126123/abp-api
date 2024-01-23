#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/BookStore.HttpApi.Host/BookStore.HttpApi.Host.csproj", "src/BookStore.HttpApi.Host/"]
COPY ["src/BookStore.Application/BookStore.Application.csproj", "src/BookStore.Application/"]
COPY ["src/BookStore.Domain/BookStore.Domain.csproj", "src/BookStore.Domain/"]
COPY ["src/BookStore.Domain.Shared/BookStore.Domain.Shared.csproj", "src/BookStore.Domain.Shared/"]
COPY ["src/BookStore.Application.Contracts/BookStore.Application.Contracts.csproj", "src/BookStore.Application.Contracts/"]
COPY ["src/BookStore.MongoDB/BookStore.MongoDB.csproj", "src/BookStore.MongoDB/"]
COPY ["src/BookStore.HttpApi/BookStore.HttpApi.csproj", "src/BookStore.HttpApi/"]
RUN dotnet restore "./src/BookStore.HttpApi.Host/./BookStore.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/src/BookStore.HttpApi.Host"
RUN dotnet build "./BookStore.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BookStore.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookStore.HttpApi.Host.dll"]