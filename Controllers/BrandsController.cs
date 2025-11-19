using DienMayLongQuyen.Api.Models;
using DienMayLongQuyen.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DienMayLongQuyen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BrandsController(AppDbContext context)
        {
            _context = context;
        }

        // GET list
        [HttpGet]
        public async Task<IActionResult> GetBrands(int page = 1, string? search = null,  int pageSize = 10)
        {
            var query = _context.Brands.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(b => b.Name.ToUpper().Contains(search.ToUpper()));
            }
    

            var totalCount = await query.CountAsync();
            var brands = await query
                .OrderBy(c => c.IndexShow)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                items = brands
            });
        }

        // GET detail
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrand(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                return NotFound(new { message = "Không tìm thấy thương hiệu" });

            return Ok(brand);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBrandDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var brand = new Brand
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive ?? true
            };

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBrand), new { id = brand.Id }, brand);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(long id, [FromBody] UpdateBrandDTO dto)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                return NotFound(new { message = "Không tìm thấy thương hiệu" });

            if (dto.Name != null) brand.Name = dto.Name;
            if (dto.Description != null) brand.Description = dto.Description;
            if (dto.IsActive != null) brand.IsActive = dto.IsActive.Value;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật thương hiệu thành công" });
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(long id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                return NotFound(new { message = "Không tìm thấy thương hiệu" });

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
