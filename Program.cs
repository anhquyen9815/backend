using DienMayLongQuyen.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Kết nối SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

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
        // Ví dụ: if (!context.Brands.Any()) { SeedData.Initialize(context); }
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
app.MapControllers();

app.Run();
