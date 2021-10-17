#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BarrierLayer/BarrierLayer.csproj", "BarrierLayer/"]
RUN dotnet restore "BarrierLayer/BarrierLayer.csproj"
COPY . .
WORKDIR "/src/BarrierLayer"
RUN dotnet build "BarrierLayer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BarrierLayer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BarrierLayer.dll"]