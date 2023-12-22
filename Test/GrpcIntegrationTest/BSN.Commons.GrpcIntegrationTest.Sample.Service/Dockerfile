#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Test/GrpcIntegrationTest/BSN.Commons.GrpcIntegrationTest.Sample.Service/BSN.Commons.GrpcIntegrationTest.Sample.Service.csproj", "Test/GrpcIntegrationTest/BSN.Commons.GrpcIntegrationTest.Sample.Service/"]
RUN dotnet restore "./Test/GrpcIntegrationTest/BSN.Commons.GrpcIntegrationTest.Sample.Service/./BSN.Commons.GrpcIntegrationTest.Sample.Service.csproj"
COPY . .
WORKDIR "/src/Test/GrpcIntegrationTest/BSN.Commons.GrpcIntegrationTest.Sample.Service"
RUN dotnet build "./BSN.Commons.GrpcIntegrationTest.Sample.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BSN.Commons.GrpcIntegrationTest.Sample.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BSN.Commons.GrpcIntegrationTest.Sample.Service.dll"]