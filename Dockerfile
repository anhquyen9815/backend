# Giai đoạn Build (Sử dụng SDK để build cả Node.js và .NET)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# 1. Thiết lập thư mục làm việc gốc (Build Context)
WORKDIR /src

# 2. COPY toàn bộ file code (backend và frontend) vào context
# Lệnh này khắc phục lỗi "not found"
COPY . .

# 3. Chạy Node.js/React Build (Bước quan trọng để tạo thư mục dist)

# a. Chạy Node.js/npm trong thư mục frontend
# ĐIỀU CHỈNH WORKDIR để chạy npm - Dùng đường dẫn tuyệt đối từ gốc: /src/fontend/website
WORKDIR /src/fontend/website
RUN npm install
RUN npm run build

# b. Quay lại thư mục build .NET
# WORKDIR này là nơi file .csproj nằm: /src/backend/DienMayLongQuyen.Api
WORKDIR /src/backend/DienMayLongQuyen.Api

# 4. Chạy lệnh Publish
# Lệnh này sẽ tự động chạy các Target trong .csproj (như đã sửa)
RUN dotnet publish "DienMayLongQuyen.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false


# Giai đoạn Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DienMayLongQuyen.Api.dll"]