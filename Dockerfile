# Giai đoạn Build (Sử dụng SDK để build cả Node.js và .NET)

# ====================================================================
# STAGE 1: node_build - Xây dựng Frontend (React)
# ====================================================================
FROM node:20-alpine AS node_build

# Thiết lập thư mục làm việc cho Frontend
WORKDIR /app/frontend

# Copy dependencies cần thiết cho Node.js từ Build Context
COPY fontend/website/package*.json ./

# Cài đặt Node Dependencies
RUN npm install

# Copy toàn bộ code Frontend và Build
COPY fontend/website .
RUN npm run build


# ====================================================================
# STAGE 2: dotnet_build - Xây dựng Backend (.NET)
# ====================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dotnet_build

# Thiết lập thư mục làm việc chính cho .NET
WORKDIR /src/backend/DienMayLongQuyen.Api

# Copy code .NET
COPY backend/DienMayLongQuyen.Api .

# Restore và Publish
RUN dotnet restore "DienMayLongQuyen.Api.csproj"
RUN dotnet publish "DienMayLongQuyen.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false


# ====================================================================
# STAGE 3: final - Môi trường Runtime
# ====================================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=dotnet_build /app/publish .
ENTRYPOINT ["dotnet", "DienMayLongQuyen.Api.dll"]