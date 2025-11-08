using DienMayLongQuyen.Api.Dtos;
using DienMayLongQuyen.Api.Models;
using DienMayLongQuyen.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DienMayLongQuyen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NewsController(AppDbContext context)
        {
            _context = context;
        }

        // GET list
        [HttpGet]
        public async Task<IActionResult> GetNews(int page = 1, int pageSize = 10)
        {
            var query = _context.News.AsQueryable();
            var totalCount = await query.CountAsync();
            var newsList = await query
                .OrderByDescending(n => n.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                items = newsList
            });
        }

        // GET detail
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsItem(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
                return NotFound(new { message = "Không tìm thấy tin tức" });

            return Ok(news);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> CreateNews([FromBody] CreateNewsDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var news = new News
            {
                Title = dto.Title,
                Content = dto.Content!,
                IsActive = dto.IsActive ?? true
            };

            _context.News.Add(news);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetNewsItem), new { id = news.Id }, news);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(int id, [FromBody] UpdateNewsDTO dto)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
                return NotFound(new { message = "Không tìm thấy tin tức" });

            if (dto.Title != null) news.Title = dto.Title;
            if (dto.Content != null) news.Content = dto.Content;
            if (dto.IsActive != null) news.IsActive = dto.IsActive.Value;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật tin tức thành công" });
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
                return NotFound(new { message = "Không tìm thấy tin tức" });

            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
