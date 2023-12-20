    # Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY . .

RUN dotnet restore .
RUN dotnet publish -c release -o /app --no-restore

# Package
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_URLS=http://*:5000
ENV APP_MESSAGE='Default message from container build'
ENTRYPOINT ["dotnet", "WebApp.dll"]