# Giai đoạn Build (Sử dụng SDK để build cả Node.js và .NET)

# ====================================================================
# STAGE 1: node_build - Xây dựng Frontend (React)
# ====================================================================
FROM node:20-alpine AS node_build

# 1. Tạo thư mục làm việc cho Frontend
WORKDIR /app/frontend

# 2. Copy dependencies cần thiết cho Node.js (đi ngược 2 cấp từ vị trí Dockerfile)
COPY ../../fontend/website/package*.json ./

# 3. Cài đặt Node Dependencies
RUN npm install

# 4. Copy toàn bộ code Frontend và Build
COPY ../../fontend/website .
RUN npm run build


# ====================================================================
# STAGE 2: dotnet_build - Xây dựng Backend (.NET)
# ====================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dotnet_build

# 1. Thiết lập thư mục làm việc chính cho .NET
# WORKDIR /src/app là vị trí tạm thời trong Docker
WORKDIR /src/app

# 2. Copy file .csproj và Restore để tối ưu Docker cache
# Copy file .csproj từ thư mục hiện tại của Dockerfile (DienMayLongQuyen.Api)
COPY DienMayLongQuyen.Api.csproj .
RUN dotnet restore "DienMayLongQuyen.Api.csproj"

# 3. Copy code .NET còn lại (Controller, Models, v.v.)
# COPY toàn bộ nội dung thư mục hiện tại (DienMayLongQuyen.Api) sang /src/app
COPY . .

# 4. Chạy lệnh Publish (Lệnh này sẽ tự động chạy các Target trong .csproj)
RUN dotnet publish "DienMayLongQuyen.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false


# ====================================================================
# STAGE 3: final - Môi trường Runtime
# ====================================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=dotnet_build /app/publish .
ENTRYPOINT ["dotnet", "DienMayLongQuyen.Api.dll"]