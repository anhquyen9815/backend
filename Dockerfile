# STAGE 1: build frontend
FROM node:20-alpine AS node_build
WORKDIR /app/fontend

# copy package.json từ repo root -> đảm bảo bạn build từ repo root
COPY fontend/website/package*.json ./
RUN npm ci
COPY fontend/website .
RUN npm run build


# STAGE 2: build .NET (publish)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dotnet_build
WORKDIR /src

# Copy only the backend project folder to avoid path confusion
# (đảm bảo build context là repo root: `docker build -f backend/DienMayLongQuyen.Api/Dockerfile .`)
COPY backend/DienMayLongQuyen.Api ./DienMayLongQuyen.Api

# Copy frontend build output into the project's wwwroot BEFORE publish
RUN mkdir -p DienMayLongQuyen.Api/wwwroot
COPY --from=node_build /app/fontend/dist ./DienMayLongQuyen.Api/wwwroot

# Restore & publish (chỉ đúng path đã copy)
WORKDIR /src/DienMayLongQuyen.Api
RUN dotnet restore "DienMayLongQuyen.Api.csproj"
RUN dotnet publish "DienMayLongQuyen.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false


# STAGE 3: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Set default listen URL (không dùng ${PORT} lúc build để tránh warning)
ENV ASPNETCORE_URLS=http://+:80

COPY --from=dotnet_build /app/publish .

# Use PORT env provided by Render at runtime (Render will map external port)
ENTRYPOINT ["dotnet", "DienMayLongQuyen.Api.dll"]
