# ====================================================================
# STAGE 1: node_build - Xây dựng Frontend (React)
# ====================================================================
FROM node:20-alpine AS node_build

WORKDIR /app/frontend
# Copy dependencies cần thiết cho Node.js
COPY fontend/website/package*.json ./
RUN npm install
# Copy toàn bộ code Frontend và Build
COPY fontend/website .
RUN npm run build


# ====================================================================
# STAGE 2: dotnet_build - Xây dựng Backend (.NET)
# ====================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dotnet_build

WORKDIR /src/app
# Copy code .NET
COPY backend/DienMayLongQuyen.Api .

RUN dotnet restore "DienMayLongQuyen.Api.csproj"

# LỆNH MỚI: COPY thư mục đã build (dist) TỪ STAGE 1 sang thư mục wwwroot
# Thư mục wwwroot được tạo trong stage hiện tại.
RUN mkdir -p wwwroot
COPY --from=node_build /app/frontend/dist ./wwwroot

# Chạy lệnh Publish
# Lệnh này sẽ thành công vì không còn kích hoạt các target npm lỗi nữa.
RUN dotnet publish "DienMayLongQuyen.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false


# ====================================================================
# STAGE 3: final - Môi trường Runtime
# ====================================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=dotnet_build /app/publish .
ENTRYPOINT ["dotnet", "DienMayLongQuyen.Api.dll"]
