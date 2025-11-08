using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DienMayLongQuyen.Api.Data
{
    public static class DatabaseInitializer
    {
        // Gọi method này sau khi build host, trước app.Run()
        public static async Task InitializeAsync(AppDbContext context, ILogger logger, int maxRetries = 5)
        {
            try
            {
                // 1) Áp migrations (MigrateAsync sẽ chờ lock nội bộ của EF Core)
                logger.LogInformation("Applying migrations...");
                await context.Database.MigrateAsync();
                logger.LogInformation("Migrations applied.");

                // 2) Tìm file triggers trong thư mục chạy (bin/...); thử các đường dẫn phổ biến
                var baseDir = AppContext.BaseDirectory; // thư mục của app 실행
                logger.LogInformation("App base directory: {baseDir}", baseDir);

                // thử một vài chỗ: Data/triggers.sql hoặc root triggers.sql
                var candidatePaths = new[]
                {
                    Path.Combine(baseDir, "Data", "triggers.sql"),
                    Path.Combine(baseDir, "triggers.sql"),
                    Path.Combine(Directory.GetCurrentDirectory(), "Data", "triggers.sql"),
                    Path.Combine(Directory.GetCurrentDirectory(), "triggers.sql")
                };

                string filePath = null;
                foreach (var p in candidatePaths)
                {
                    logger.LogDebug("Checking for triggers file: {p}", p);
                    if (File.Exists(p))
                    {
                        filePath = p;
                        break;
                    }
                }

                if (filePath == null)
                {
                    logger.LogWarning("⚠️ triggers.sql not found in expected locations. Skipping trigger initialization. Looked at: {paths}",
                        string.Join("; ", candidatePaths));
                    return;
                }

                logger.LogInformation("Found triggers file: {filePath}", filePath);

                // 3) Đọc nội dung file
                var sql = await File.ReadAllTextAsync(filePath);
                if (string.IsNullOrWhiteSpace(sql))
                {
                    logger.LogWarning("triggers.sql is empty. Nothing to run.");
                    return;
                }

                // 4) Thực thi SQL với retry nếu gặp lock (SQLite có thể trả lỗi lock nếu DB đang được sử dụng)
                var attempt = 0;
                while (true)
                {
                    try
                    {
                        attempt++;
                        logger.LogInformation("Executing triggers (attempt {attempt})...", attempt);
                        // dùng transaction để an toàn (SQLite hỗ trợ)
                        await using var tx = await context.Database.BeginTransactionAsync();
                        await context.Database.ExecuteSqlRawAsync(sql);
                        await tx.CommitAsync();
                        logger.LogInformation("Triggers executed successfully.");
                        break;
                    }
                    catch (DbUpdateException dbEx)
                    {
                        logger.LogWarning(dbEx, "DbUpdateException while executing triggers (attempt {attempt}).", attempt);
                        if (attempt >= maxRetries) throw;
                        await Task.Delay(1000 * attempt); // backoff
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to execute triggers.");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database initialization failed.");
            }
        }
    }
}
