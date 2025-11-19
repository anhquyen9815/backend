# ====================================================================
# STAGE 1: node_build - Xây dựng Frontend (React)
# Sử dụng image chính thức của Node.js để có npm
# ====================================================================
FROM node:20-alpine AS node_build

# 1. Thiết lập thư mục làm việc và copy file package
WORKDIR /app/fontend/website

# 2. Copy dependencies cần thiết cho Node.js từ Build Context (thư mục gốc repo)
# Đường dẫn: fontend/website/ là tương đối so với Build Context
COPY fontend/website/package*.json ./

# 3. Cài đặt Node Dependencies
RUN npm install

# 4. Copy toàn bộ code Frontend và Build
COPY fontend/website .
RUN npm run build


# ====================================================================
# STAGE 2: dotnet_build - Xây dựng Backend (.NET)
# Sử dụng image SDK của .NET để có dotnet publish
# ====================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dotnet_build

# 1. Thiết lập thư mục làm việc chính cho .NET
WORKDIR /src/backend/DienMayLongQuyen.Api

# 2. Copy code .NET và restore
# Copy các file của dự án .NET (backend/DienMayLongQuyen.Api)
COPY backend/DienMayLongQuyen.Api .
RUN dotnet restore "DienMayLongQuyen.Api.csproj"

# 3. Chạy lệnh Publish
# Lệnh này sẽ tự động chạy các Target trong .csproj (bao gồm việc COPY file đã build từ STAGE 1 sang wwwroot)
RUN dotnet publish "DienMayLongQuyen.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false


# ====================================================================
# STAGE 3: final - Môi trường Runtime
# Sử dụng image ASP.NET nhỏ gọn để chạy ứng dụng
# ====================================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
# Copy file đã publish từ Stage 2
COPY --from=dotnet_build /app/publish .
ENTRYPOINT ["dotnet", "DienMayLongQuyen.Api.dll"]