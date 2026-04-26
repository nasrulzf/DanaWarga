FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY DanaWarga.slnx ./
COPY src ./src
COPY tests ./tests

RUN dotnet restore src/DanaWarga.WebMvc/DanaWarga.WebMvc.csproj
RUN dotnet publish src/DanaWarga.WebMvc/DanaWarga.WebMvc.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "DanaWarga.WebMvc.dll"]