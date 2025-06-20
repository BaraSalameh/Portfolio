# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy and restore
COPY . . 
RUN dotnet restore

# Build and publish
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Tell ASP.NET to listen on Render's required port
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Replace with your actual project DLL name
ENTRYPOINT ["dotnet", "Portfolio.dll"]
