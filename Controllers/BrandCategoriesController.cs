using DienMayLongQuyen.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DienMayLongQuyen.Api.Data;
using DienMayLongQuyen.Api.Models;

namespace DienMayLongQuyen.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BrandCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // ==========================
        // GET: api/BrandCategories
        // ==========================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandCategory>>> GetBrandCategories()
        {
            var list = await _context.BrandCategories
                .Include(bc => bc.Brand)
                .Include(bc => bc.Category)
                .ToListAsync();

            return Ok(list);
        }

        // ==========================
        // GET: api/BrandCategories/{id}
        // ==========================
        [HttpGet("{id}")]
        public async Task<ActionResult<BrandCategory>> GetBrandCategory(int id)
        {
            var brandCategory = await _context.BrandCategories
                .Include(bc => bc.Brand)
                .Include(bc => bc.Category)
                .FirstOrDefaultAsync(bc => bc.Id == id);

            if (brandCategory == null)
            {
                return NotFound(new { message = "Không tìm thấy BrandCategory." });
            }

            return Ok(brandCategory);
        }

        // ==========================
        // POST: api/BrandCategories
        // ==========================
        [HttpPost]
        public async Task<ActionResult<BrandCategory>> CreateBrandCategory(BrandCategory model)
        {
            // Kiểm tra trùng (BrandId + CategoryId)
            var exists = await _context.BrandCategories
                .AnyAsync(bc => bc.BrandId == model.BrandId && bc.CategoryId == model.CategoryId);

            if (exists)
            {
                return BadRequest(new { message = "Cặp Brand và Category này đã tồn tại." });
            }

            _context.BrandCategories.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBrandCategory), new { id = model.Id }, model);
        }

        // ===========================
        // POST: api/BrandCategories/bulk-insert
        // ===========================
        [HttpPost("bulk-insert")]
        public async Task<IActionResult> BulkInsertBrandCategories([FromBody] List<CreateBrandCategoryDTO> brandCategories)
        {
            if (brandCategories == null || !brandCategories.Any())
                return BadRequest("Danh sách rỗng.");

            // Lấy tất cả cặp BrandId-CategoryId hiện có trong DB
            var existingPairs = await _context.BrandCategories
                .Select(bc => new { bc.BrandId, bc.CategoryId })
                .ToListAsync();

            // Lọc ra những bản ghi chưa tồn tại
            var newEntities = brandCategories
                .Where(dto => !existingPairs.Any(e => e.BrandId == dto.BrandId && e.CategoryId == dto.CategoryId))
                .Select(dto => new BrandCategory
                {
                    BrandId = dto.BrandId,
                    CategoryId = dto.CategoryId
                })
                .ToList();

            // Nếu tất cả đều trùng thì báo luôn
            if (!newEntities.Any())
            {
                return Conflict(new
                {
                    message = "Tất cả các cặp Brand - Category đều đã tồn tại, không có gì để thêm."
                });
            }

            // Thêm các bản ghi mới
            await _context.BrandCategories.AddRangeAsync(newEntities);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Đã thêm {newEntities.Count} BrandCategory mới, {brandCategories.Count - newEntities.Count} bản ghi bị trùng.",
                // added = newEntities,
                insertedCount = newEntities.Count,
                skippedCount = brandCategories.Count - newEntities.Count,
                inserted = newEntities
            });
        }


        // ==========================
        // PUT: api/BrandCategories/{id}
        // ==========================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrandCategory(int id, BrandCategory model)
        {
            if (id != model.Id)
            {
                return BadRequest(new { message = "Id không khớp." });
            }

            var existing = await _context.BrandCategories.FindAsync(id);
            if (existing == null)
            {
                return NotFound(new { message = "Không tìm thấy BrandCategory." });
            }

            // Kiểm tra trùng BrandId + CategoryId (ngoại trừ bản ghi hiện tại)
            var duplicate = await _context.BrandCategories
                .AnyAsync(bc => bc.Id != id && bc.BrandId == model.BrandId && bc.CategoryId == model.CategoryId);

            if (duplicate)
            {
                return BadRequest(new { message = "Cặp Brand và Category này đã tồn tại." });
            }

            existing.BrandId = model.BrandId;
            existing.CategoryId = model.CategoryId;

            _context.Entry(existing).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // ==========================
        // DELETE: api/BrandCategories/{id}
        // ==========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrandCategory(int id)
        {
            var brandCategory = await _context.BrandCategories.FindAsync(id);
            if (brandCategory == null)
            {
                return NotFound(new { message = "Không tìm thấy BrandCategory." });
            }

            _context.BrandCategories.Remove(brandCategory);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa thành công." });
        }

        // GET: api/BrandCategories/filter
        [HttpGet("filter")]
        public async Task<ActionResult> GetFilteredBrandCategories(
            [FromQuery] int? page,
            [FromQuery] int? size,
            [FromQuery] int? brandId,
            [FromQuery] int? categoryId)
        {
            try
            {
                var query = _context.BrandCategories
                    .Include(bc => bc.Brand)
                    .Include(bc => bc.Category)
                    .AsQueryable();

                // Lọc theo BrandId
                if (brandId.HasValue && brandId > 0)
                {
                    query = query.Where(bc => bc.BrandId == brandId);
                }

                // Lọc theo CategoryId
                if (categoryId.HasValue && categoryId > 0)
                {
                    query = query.Where(bc => bc.CategoryId == categoryId);
                }

                // Tổng số bản ghi
                var totalCount = await query.CountAsync();

                // Phân trang
                int pageSize = size ?? 10;
                int pageNumber = page ?? 1;

                var data = await query
                    // .OrderByDescending(bc => bc.Id)
                    .OrderBy(bc => bc.Brand != null ? bc.Brand.IndexShow : bc.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(bc => new
                    {
                        bc.Id,
                        bc.BrandId,
                        BrandName = bc.Brand != null ? bc.Brand.Name : null,
                        bc.CategoryId,
                        CategoryName = bc.Category != null ? bc.Category.Name : null
                    })
                    .ToListAsync();

                return Ok(new
                {
                    total = totalCount,
                    page = pageNumber,
                    size = pageSize,
                    items = data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy BrandCategory có filter: " + ex.Message);
                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }

    }


}
