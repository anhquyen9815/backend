# Giai đoạn Build (Sử dụng SDK để build cả Node.js và .NET)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# 1. Thiết lập thư mục làm việc chính
# Trong Docker, code của anh được COPY vào thư mục này, ví dụ: /src/DienMayLongQuyen.Api
WORKDIR /src/DienMayLongQuyen.Api

# 2. Copy file .csproj (để tối ưu hóa cache của Docker)
COPY ["DienMayLongQuyen.Api.csproj", "."]

# 3. Chạy Node.js/React Build (Bước quan trọng để tạo thư mục dist)

# a. Copy toàn bộ code Frontend
# Ta biết thư mục fontend/website nằm ở 2 cấp trên so với thư mục làm việc hiện tại
COPY ../../fontend/website /src/fontend/website

# b. Chạy Node.js/npm trong thư mục frontend
# ĐIỀU CHỈNH WORKDIR để chạy npm
WORKDIR /src/fontend/website
RUN npm install
RUN npm run build

# c. Quay lại thư mục build .NET
# WorkDir này phải khớp với nơi file .csproj đã được copy (bước 2)
WORKDIR /src/DienMayLongQuyen.Api

# 4. Copy phần code .NET còn lại
COPY . .

# 5. Chạy lệnh Publish
# Lệnh này sẽ tự động chạy các Target trong .csproj (như đã sửa)
RUN dotnet publish "DienMayLongQuyen.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false


# Giai đoạn Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DienMayLongQuyen.Api.dll"]