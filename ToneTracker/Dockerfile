FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ToneTracker/ToneTracker.csproj", "ToneTracker/"]
RUN dotnet restore "ToneTracker/ToneTracker.csproj"
COPY . .
WORKDIR "/src/ToneTracker"
RUN dotnet build "ToneTracker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToneTracker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToneTracker.dll"]
