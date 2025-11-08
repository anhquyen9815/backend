# ----- build stage -----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore first (cache layer)
COPY ["DienMayLongQuyen.Api/DienMayLongQuyen.Api.csproj", "DienMayLongQuyen.Api/"]
# nếu bạn có solution, copy .sln tương ứng
# COPY ["YourSolution.sln", "."]
RUN dotnet restore "DienMayLongQuyen.Api/DienMayLongQuyen.Api.csproj"

# copy everything else and build
COPY . .
WORKDIR "/src/DienMayLongQuyen.Api"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ----- runtime stage -----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published app
COPY --from=build /app/publish ./

# tạo thư mục data cho Sqlite nếu cần
RUN mkdir -p /app/Data
VOLUME [ "/app/Data" ]

# set environment (optional)
ENV ASPNETCORE_URLS=http://+:5000
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 5000

# Start app
ENTRYPOINT ["dotnet", "DienMayLongQuyen.Api.dll"]
