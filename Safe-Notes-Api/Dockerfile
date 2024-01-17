FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Safe-Notes-Api.csproj", "."]
RUN dotnet restore "./Safe-Notes-Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Safe-Notes-Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Safe-Notes-Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Safe-Notes-Api.dll"]