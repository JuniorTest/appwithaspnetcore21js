#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:2.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:2.1 AS build
WORKDIR /src
COPY ["SpyStore.Service/SpyStore.Service.csproj", "SpyStore.Service/"]
COPY ["SpyStore.Dal/SpyStore.Dal.csproj", "SpyStore.Dal/"]
COPY ["SpyStore.Models/SpyStore.Models.csproj", "SpyStore.Models/"]
RUN dotnet restore "SpyStore.Service/SpyStore.Service.csproj"
COPY . .
WORKDIR "/src/SpyStore.Service"
RUN dotnet build "SpyStore.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpyStore.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpyStore.Service.dll"]
