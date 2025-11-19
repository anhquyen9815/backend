# ====================================================================
# STAGE 1: node_build - Xây dựng Frontend (React)
# ====================================================================
FROM node:20-alpine AS node_build

# 1. Thiết lập WORKDIR và copy file package
WORKDIR /app/frontend
# COPY package.json từ thư mục frontend/website
COPY fontend/website/package*.json ./

# 2. Cài đặt Dependencies
RUN npm install

# 3. Copy code Frontend và Build
COPY fontend/website .
RUN npm run build


# ====================================================================
# STAGE 2: dotnet_build - Xây dựng Backend (.NET)
# ====================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dotnet_build

# 1. Thiết lập WORKDIR cho .NET
WORKDIR /src/backend/DienMayLongQuyen.Api

# 2. Copy file .csproj và restore để tối ưu Docker cache
# Copy file .csproj từ thư mục gốc của repo
COPY backend/DienMayLongQuyen.Api/DienMayLongQuyen.Api.csproj .
RUN dotnet restore "DienMayLongQuyen.Api.csproj"

# 3. Copy code còn lại (bao gồm Controllers, Models, Data, v.v.)
# COPY toàn bộ thư mục backend/DienMayLongQuyen.Api
COPY backend/DienMayLongQuyen.Api .

# 4. Chạy lệnh Publish (Lệnh này sẽ chạy các Target trong .csproj)
RUN dotnet publish "DienMayLongQuyen.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false


# ====================================================================
# STAGE 3: final - Môi trường Runtime
# ====================================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=dotnet_build /app/publish .
ENTRYPOINT ["dotnet", "DienMayLongQuyen.Api.dll"]


### ✅ Hành động cần làm:

1.  **Cập nhật `Dockerfile`:** Thay thế file `Dockerfile` của anh bằng nội dung trên.
2.  **Đẩy code:** Đẩy code đã sửa (cả `Dockerfile` và `.csproj` đã được sửa đường dẫn tuyệt đối) lên Git.

Việc này sẽ khiến Docker tính toán checksum của từng file riêng biệt và khắc phục lỗi `not found` bằng cách sử dụng nhiều lệnh `COPY` rõ ràng hơn. Chúc anh triển khai thành công!