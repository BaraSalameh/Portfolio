# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy everything and restore dependencies
COPY . . 
RUN dotnet restore

# Build and publish the app
RUN dotnet publish -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Tell ASP.NET to listen on port 10000 (Render default)
ENV ASPNETCORE_URLS=http://+:10000

EXPOSE 10000
ENTRYPOINT ["dotnet", "Portfolio.dll"]
