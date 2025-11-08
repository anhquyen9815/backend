using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DienMayLongQuyen.Api.Data
{
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Khởi tạo DB: (tuỳ chọn) áp migrations, rồi chạy triggers.sql (idempotent).
        /// </summary>
        public static async Task InitializeAsync(
            AppDbContext context,
            ILogger logger,
            bool applyMigrations = false,
            int maxRetries = 5)
        {
            try
            {
                // 1) Tuỳ chọn: áp migrations (để Program.cs kiểm soát)
                if (applyMigrations)
                {
                    logger.LogInformation("Applying migrations...");
                    await context.Database.MigrateAsync();
                    logger.LogInformation("Migrations applied.");
                }
                else
                {
                    logger.LogInformation("Skipping migrations (applyMigrations = false).");
                }

                // 2) Tìm triggers.sql
                var baseDir = AppContext.BaseDirectory;
                logger.LogInformation("App base directory: {baseDir}", baseDir);

                var candidatePaths = new[]
                {
                    Path.Combine(baseDir, "Data", "triggers.sql"),
                    Path.Combine(baseDir, "triggers.sql"),
                    Path.Combine(Directory.GetCurrentDirectory(), "Data", "triggers.sql"),
                    Path.Combine(Directory.GetCurrentDirectory(), "triggers.sql")
                };

                string? filePath = null;
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
                    logger.LogWarning("triggers.sql not found. Skipping trigger initialization. Looked at: {paths}",
                        string.Join("; ", candidatePaths));
                    return;
                }

                logger.LogInformation("Found triggers file: {filePath}", filePath);

                // 3) Đọc nội dung
                var sql = await File.ReadAllTextAsync(filePath);
                if (string.IsNullOrWhiteSpace(sql))
                {
                    logger.LogWarning("triggers.sql is empty. Nothing to run.");
                    return;
                }

                // 4) Thực thi SQL với retry (SQLite có thể lock)
                for (int attempt = 1; attempt <= maxRetries; attempt++)
                {
                    try
                    {
                        logger.LogInformation("Executing triggers (attempt {attempt})...", attempt);
                        await using var tx = await context.Database.BeginTransactionAsync();
                        await context.Database.ExecuteSqlRawAsync(sql);
                        await tx.CommitAsync();
                        logger.LogInformation("Triggers executed successfully.");
                        return;
                    }
                    catch (DbUpdateException dbEx)
                    {
                        logger.LogWarning(dbEx, "DbUpdateException while executing triggers (attempt {attempt}).");
                        if (attempt == maxRetries) throw;
                        await Task.Delay(1000 * attempt); // backoff
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
