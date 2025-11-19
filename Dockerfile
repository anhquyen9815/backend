# syntax=docker/dockerfile:1

### build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore to leverage cache
COPY ["DienMayLongQuyen.Api.csproj", "./"]
RUN dotnet restore "./DienMayLongQuyen.Api.csproj"

# copy everything else and publish
COPY . .
RUN dotnet publish "./DienMayLongQuyen.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

### runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# copy published app
COPY --from=build /app/publish ./

# copy SEED database from repo into container
COPY Seed/longquyen.db /app/Seed/longquyen.db

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "DienMayLongQuyen.Api.dll"]
