FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Инструкции для себя:

# 1. Копируем файлы всех проектов решения, чтобы восстановить зависимости
COPY ["src/CsvMetricsProcessorService.API/CsvMetricsProcessorService.API.csproj", "src/CsvMetricsProcessorService.API/"]
COPY ["src/CsvMetricsProcessorService.Domain/CsvMetricsProcessorService.Domain.csproj", "src/CsvMetricsProcessorService.Domain/"]
COPY ["src/CsvMetricsProcessorService.Application/CsvMetricsProcessorService.Application.csproj", "src/CsvMetricsProcessorService.Application/"]
COPY ["src/CsvMetricsProcessorService.Infrastructure/CsvMetricsProcessorService.Infrastructure.csproj", "src/CsvMetricsProcessorService.Infrastructure/"]

# 2. Запускаем восстановление пакетов для главного проекта (оно подтянет и остальные)
RUN dotnet restore "src/CsvMetricsProcessorService.API/CsvMetricsProcessorService.API.csproj"

# 3. Копируем вообще весь исходный код в контейнер
COPY . .

# 4. Собираем проект
WORKDIR "/src/src/CsvMetricsProcessorService.API"
RUN dotnet build "./CsvMetricsProcessorService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CsvMetricsProcessorService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CsvMetricsProcessorService.API.dll"]
