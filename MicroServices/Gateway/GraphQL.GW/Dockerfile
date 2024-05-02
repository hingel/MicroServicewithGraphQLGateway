#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MicroServices/Gateway/GraphQL.GW/GraphQL.GW.csproj", "MicroServices/Gateway/GraphQL.GW/"]
COPY ["MicroServices/APIs/User/User.Db/User.Db.csproj", "MicroServices/APIs/User/User.Db/"]
COPY ["MicroServices/APIs/Service/Service.Db/Service.Db.csproj", "MicroServices/APIs/Service/Service.Db/"]
RUN dotnet restore "./MicroServices/Gateway/GraphQL.GW/./GraphQL.GW.csproj"
COPY . .
WORKDIR "/src/MicroServices/Gateway/GraphQL.GW"
RUN dotnet build "./GraphQL.GW.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GraphQL.GW.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GraphQL.GW.dll"]