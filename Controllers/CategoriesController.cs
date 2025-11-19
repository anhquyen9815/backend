using DienMayLongQuyen.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DienMayLongQuyen.Api.Data; 

namespace DienMayLongQuyen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // =============================
        // GET /api/categories?page=1&pageSize=10
        // =============================
        [HttpGet]
        public async Task<IActionResult> GetCategories(int page = 1, int pageSize = 10)
        {
            var query = _context.Categories.AsQueryable();
            query = query.Where(c => c.IsActive == true);

            var totalCount = await query.CountAsync();
            var categories = await query
                .OrderBy(c => c.IndexShow)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                items = categories
            });
        }

        // =============================
        // GET /api/categories/{id}
        // =============================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound(new { message = "Không tìm thấy danh mục" });

            return Ok(category);
        }

        // =============================
        // POST /api/categories
        // =============================
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                Name = dto.Name,
                Slug = dto.Slug ?? string.Empty,
                ParentId = dto.ParentId,
                IsActive = dto.IsActive ?? true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // =============================
        // PUT /api/categories/{id}
        // =============================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDTO dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new { message = "Không tìm thấy danh mục" });

            if (dto.Name != null) category.Name = dto.Name;
            if (dto.Slug != null) category.Slug = dto.Slug;
            if (dto.ParentId != null) category.ParentId = dto.ParentId;
            if (dto.IsActive != null) category.IsActive = dto.IsActive.Value;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật danh mục thành công" });
        }

        // =============================
        // DELETE /api/categories/{id}
        // =============================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound(new { message = "Không tìm thấy danh mục" });

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
