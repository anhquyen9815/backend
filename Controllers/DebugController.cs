using DienMayLongQuyen.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace DienMayLongQuyen.Api.Controllers
{
    [Route("debug")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly AppDbContext _db; // ← đổi tên DbContext

        public DebugController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("db-columns")]
        public async Task<IActionResult> GetProductColumns()
        {
            var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "PRAGMA table_info('Products');";

            var reader = await cmd.ExecuteReaderAsync();

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
    }
}
