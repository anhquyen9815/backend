using DienMayLongQuyen.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System;

// Top-level app
var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURE SQLITE RUNTIME PATH & SEED COPY LOGIC ------------------
// runtime DB path (bạn có thể set env var SQLITE_DB_PATH nếu muốn khác)
var dbPath = Environment.GetEnvironmentVariable("SQLITE_DB_PATH")
             ?? Path.Combine(builder.Environment.ContentRootPath, "app.db");

// seed DB path inside image (Dockerfile phải copy Seed/longquyen.db vào image)
var seedPath = Path.Combine(builder.Environment.ContentRootPath, "Seed", "longquyen.db");

// create directory for runtime db if needed
var dbDir = Path.GetDirectoryName(dbPath);
if (!string.IsNullOrEmpty(dbDir) && !Directory.Exists(dbDir))
{
    Directory.CreateDirectory(dbDir);
}

// decide whether to force overwrite the runtime DB with seed
var forceSeed = (Environment.GetEnvironmentVariable("FORCE_SEED") ?? "false").ToLower() == "true";

if (File.Exists(seedPath))
{
    try
    {
        if (forceSeed)
        {
            File.Copy(seedPath, dbPath, overwrite: true);
            Console.WriteLine($"[DB SEED] FORCE: copied seed DB '{seedPath}' -> '{dbPath}'");
        }
        else
        {
            if (!File.Exists(dbPath))
            {
                File.Copy(seedPath, dbPath);
                Console.WriteLine($"[DB SEED] copied seed DB '{seedPath}' -> '{dbPath}'");
            }
            else
            {
                Console.WriteLine($"[DB SEED] runtime DB exists at '{dbPath}', skip copy");
            }
        }

        // Attempt to set permissive permissions on Linux so the app can write
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
        {
            try
            {
                var p = System.Diagnostics.Process.Start("chmod", $"666 \"{dbPath}\"");
                p?.WaitForExit();
            }
            catch
            {
                // ignore chmod failure
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[DB SEED] Copy failed: {ex.Message}");
    }
}
else
{
    Console.WriteLine($"[DB SEED] Seed DB not found at '{seedPath}'. Make sure Seed/longquyen.db exists in the repo and Dockerfile copies it.");
}
// --------------------------------------------------------------------

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Kết nối SQLite — dùng dbPath đã chuẩn bị ở trên
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// CORS cho React (http://localhost:5173 hoặc http://localhost:3000)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.WriteIndented = true;
    });

var app = builder.Build();

// Thêm UseStaticFiles cho thư mục wwwroot mặc định
app.UseStaticFiles();

// --- ÁP MIGRATIONS VÀ CHẠY INITIALIZER (CHỈ 1 NƠI) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        // Chỉ auto-apply migrations trong Development
        var applyMigrations = app.Environment.IsDevelopment();

        if (applyMigrations)
        {
            logger.LogInformation("Environment is Development: migrations will be applied by DatabaseInitializer.");
        }
        else
        {
            logger.LogInformation("Environment is Production/non-Development: migrations will NOT be auto-applied.");
        }

        // DatabaseInitializer xử lý: (tuỳ chọn) apply migrations và chạy triggers
        await DatabaseInitializer.InitializeAsync(context, logger, applyMigrations);

        // Seed dữ liệu idempotent — đảm bảo SeedData.Initialize kiểm tra tồn tại trước khi insert
        SeedData.Initialize(context);
    }
    catch (Exception ex)
    {
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var log = loggerFactory.CreateLogger<Program>();
        log.LogError(ex, "An error occurred while initializing the database.");
        // Tuỳ chọn: throw; // để app không start nếu DB init thất bại
    }
}
// ---------------------------------------------------------------

// Cấu hình pipeline
// Cho phép bật Swagger tạm qua biến môi trường ENABLE_SWAGGER=true
var enableSwagger = builder.Configuration.GetValue<bool>("ENABLE_SWAGGER", false);

if (enableSwagger || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

// BẮT ĐẦU CẤU HÌNH ROUTING ĐẦY ĐỦ CHO SPA FALLBACK
app.UseRouting();
app.MapControllers();

// *** DÒNG GIẢI QUYẾT LỖI 404 KHI RELOAD ***
app.MapFallbackToFile("index.html");

app.Run();
