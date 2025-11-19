using DienMayLongQuyen.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace DienMayLongQuyen.Api.Controllers
{
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DebugController(AppDbContext db)
        {
            _db = db;
        }

        // route tuyệt đối để tránh prefix hoặc route convention khác
        [AllowAnonymous]
        [HttpGet("/debug/db-columns")]
        public async Task<IActionResult> GetProductColumns()
        {
            try
            {
                var conn = _db.Database.GetDbConnection();
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "PRAGMA table_info('Products');";

                using var reader = await cmd.ExecuteReaderAsync();

                var columns = new List<object>();

                while (await reader.ReadAsync())
                {
                    columns.Add(new
                    {
                        cid = reader["cid"],
                        name = reader["name"],
                        type = reader["type"]
                    });
                }

                await conn.CloseAsync();
                return Ok(columns);
            }
            catch (Exception ex)
            {
                // trả lỗi rõ ràng để debug trên Render logs / response
                return Problem(detail: ex.ToString(), statusCode: 500);
            }
        }
    }
}
