using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DienMayLongQuyen.Api.Data;
using DienMayLongQuyen.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DienMayLongQuyen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarrantiesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public WarrantiesController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/warranties
        // params: search (name), isActive (bool), page, pageSize, sortBy (indexShow, periodMonths)
        [HttpGet]
        public async Task<IActionResult> GetList(
            [FromQuery] string? search,
            [FromQuery] bool? isActive,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? sortBy = "indexShow")
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;

            var q = _db.Warranties.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(w => EF.Functions.Like(w.Name, $"%{s}%"));
            }

            if (isActive.HasValue)
                q = q.Where(w => w.IsActive == isActive.Value);

            // sorting
            q = sortBy?.ToLower() switch
            {
                "periodmonths" => q.OrderBy(w => w.PeriodMonths).ThenBy(w => w.IndexShow),
                "name" => q.OrderBy(w => w.Name),
                _ => q.OrderBy(w => w.IndexShow).ThenBy(w => w.Id)
            };

            var total = await q.CountAsync();
            var items = await q
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                total,
                page,
                pageSize,
                items
            };

            return Ok(result);
        }

        // GET: api/warranties/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var w = await _db.Warranties.FindAsync(id);
            if (w == null) return NotFound();
            return Ok(w);
        }

        // POST: api/warranties
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWarrantyDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var w = new Warranty
            {
                Name = dto.Name!.Trim(),
                Description = dto.Description,
                PeriodMonths = dto.PeriodMonths ?? 12,
                IndexShow = dto.IndexShow ?? 0,
                IsActive = dto.IsActive ?? true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Warranties.Add(w);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = w.Id }, w);
        }

        // POST: api/warranties/bulk
        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBulk([FromBody] List<CreateWarrantyDTO> list)
        {
            if (list == null || list.Count == 0) return BadRequest("Empty payload");

            var now = DateTime.UtcNow;
            var entities = list.Select(dto => new Warranty
            {
                Name = dto.Name?.Trim() ?? string.Empty,
                Description = dto.Description,
                PeriodMonths = dto.PeriodMonths ?? 12,
                IndexShow = dto.IndexShow ?? 0,
                IsActive = dto.IsActive ?? true,
                CreatedAt = now,
                UpdatedAt = now
            }).ToList();

            _db.Warranties.AddRange(entities);
            await _db.SaveChangesAsync();

            return Ok(new { created = entities.Count });
        }

        

        // PUT: api/warranties/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWarrantyDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var w = await _db.Warranties.FindAsync(id);
            if (w == null) return NotFound();

            w.Name = dto.Name?.Trim() ?? w.Name;
            w.Description = dto.Description ?? w.Description;
            w.PeriodMonths = dto.PeriodMonths ?? w.PeriodMonths;
            w.IndexShow = dto.IndexShow ?? w.IndexShow;
            w.IsActive = dto.IsActive ?? w.IsActive;
            w.UpdatedAt = DateTime.UtcNow;

            _db.Warranties.Update(w);
            await _db.SaveChangesAsync();

            return Ok(w);
        }

        // DELETE: api/warranties/{id}
        // We'll soft-delete by default: set IsActive=false
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool hard = false)
        {
            var w = await _db.Warranties.FindAsync(id);
            if (w == null) return NotFound();

            if (hard)
            {
                _db.Warranties.Remove(w);
            }
            else
            {
                w.IsActive = false;
                w.UpdatedAt = DateTime.UtcNow;
                _db.Warranties.Update(w);
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }

    }
}
