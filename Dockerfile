# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["wt-lab4-4mvc-ostashevskaya.csproj", "./"]
RUN dotnet restore "wt-lab4-4mvc-ostashevskaya.csproj"

COPY . .
RUN dotnet publish "wt-lab4-4mvc-ostashevskaya.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "wt-lab4-4mvc-ostashevskaya.dll"]
