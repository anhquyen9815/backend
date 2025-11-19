using DienMayLongQuyen.Api.Models;
using DienMayLongQuyen.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace DienMayLongQuyen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET list có phân trang
        [HttpGet]
        public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 20)
        {
            var query = _context.Products.AsQueryable();
            var totalCount = await query.CountAsync();
            var products = await query
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                items = products
            });
        }

        // GET detail
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound(new { message = "Không tìm thấy sản phẩm" });

            return Ok(product);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                Name = dto.Name ?? string.Empty,
                Code = dto.Code ?? string.Empty,
                Slug = dto.Slug ?? string.Empty,
                Image = dto.Image ?? string.Empty,
                Description = dto.Description ?? string.Empty,
                Price = dto.Price ?? 0,
                DiscountPrice = dto.DiscountPrice ?? 0,
                CategoryId = dto.CategoryId,
                BrandId = dto.BrandId,
                IsActive = dto.IsActive ?? true
            };

            _context.Products.Add(product);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqliteException sqlEx && sqlEx.SqliteErrorCode == 19)
            {
                // UNIQUE constraint failed
                return Conflict(new { message = "Code sản phẩm đã tồn tại" });
            }
            catch
            {
                return StatusCode(500, new { message = "Đã có lỗi xảy ra" });
            }

        }

        // POST: api/products/bulk-insert
        [HttpPost("bulk-insert")]
        public async Task<IActionResult> BulkInsertProducts([FromBody] List<CreateProductDTO> products)
        {
            if (products == null || products.Count == 0)
                return BadRequest(new { message = "Danh sách sản phẩm trống" });

            var inserted = new List<Product>();
            var skipped = new List<object>();

            foreach (var dto in products)
            {
                if (string.IsNullOrWhiteSpace(dto.Code))
                {
                    skipped.Add(new { dto.Code, dto.Name, reason = "Thiếu mã sản phẩm" });
                    continue;
                }

                // Kiểm tra trùng Code
                bool exists = await _context.Products.AnyAsync(p => p.Code == dto.Code);
                if (exists)
                {
                    skipped.Add(new { dto.Code, dto.Name, reason = "Trùng mã sản phẩm" });
                    continue;
                }

                var product = new Product
                {
                    Name = dto.Name ?? string.Empty,
                    Code = dto.Code ?? string.Empty,
                    Slug = dto.Slug ?? string.Empty,
                    Image = dto.Image ?? string.Empty,
                    Gallery = dto.Gallery ?? string.Empty,
                    Description = dto.Description ?? string.Empty,
                    Price = dto.Price ?? 0,
                    DiscountPrice = dto.DiscountPrice ?? 0,
                    DiscountPercent = dto.DiscountPercent ?? 0,
                    CategoryId = dto.CategoryId,
                    BrandId = dto.BrandId,
                    IsActive = dto.IsActive ?? true
                };

                _context.Products.Add(product);
                inserted.Add(product);
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Hoàn tất thêm danh sách sản phẩm",
                    insertedCount = inserted.Count,
                    skippedCount = skipped.Count,
                    inserted = inserted.Select(p => new { p.Code, p.Name }),
                    skipped
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lưu dữ liệu", error = ex.Message });
            }
        }


        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { message = "Không tìm thấy sản phẩm" });

            if (dto.Name != null) product.Name = dto.Name;
            if (dto.Code != null) product.Name = dto.Code;
            if (dto.Slug != null) product.Name = dto.Slug;
            if (dto.Image != null) product.Name = dto.Image;
            if (dto.Description != null) product.Description = dto.Description;
            if (dto.DiscountPrice != null) product.DiscountPrice = dto.DiscountPrice.Value;
            if (dto.DiscountPercent != null) product.DiscountPercent = dto.DiscountPercent.Value;
            if (dto.Price != null) product.Price = dto.Price.Value;
            if (dto.CategoryId != null) product.CategoryId = dto.CategoryId;
            if (dto.BrandId != null) product.BrandId = dto.BrandId;
            if (dto.IsActive != null) product.IsActive = dto.IsActive.Value;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật sản phẩm thành công" });
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { message = "Không tìm thấy sản phẩm" });

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<ActionResult> GetFilteredProducts(
            [FromQuery] int? size,
            [FromQuery] int? page,
            [FromQuery] string? keysearch,
            [FromQuery] int? brandId,
            [FromQuery] int? categoryId,
            [FromQuery] SortField? sortBy,
            [FromQuery] SortOrder? sortOrder,
            [FromQuery] string? attributeFilters,
            [FromQuery] double? minPrice,
            [FromQuery] double? maxPrice
            )
        {
            try
            {
                var query = _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .AsQueryable();



                if (!string.IsNullOrWhiteSpace(keysearch))
                {
                    query = query.Where(p =>
                        p.Name.Contains(keysearch) ||
                        p.Code.Contains(keysearch));
                }

                if (brandId.HasValue && brandId > 0)
                {
                    query = query.Where(p => p.BrandId == brandId);
                }

                if (categoryId.HasValue && categoryId > 0)
                {
                    query = query.Where(p => p.CategoryId == categoryId);
                }

                if (minPrice.HasValue)
                    query = query.Where(p => p.DiscountPrice >= minPrice);

                if (maxPrice.HasValue)
                    query = query.Where(p => p.DiscountPrice <= maxPrice);

                List<int> optionIds = new();
                if (!string.IsNullOrWhiteSpace(attributeFilters))
                {
                    foreach (var part in attributeFilters
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Trim()))
                    {
                        if (int.TryParse(part, out var id))
                            optionIds.Add(id);
                        else
                            return BadRequest(new { message = $"Invalid attribute id '{part}'." });
                    }

                    optionIds = optionIds.Distinct().ToList();
                }
                // Filter theo AttributeOptionId
                if (optionIds.Any())
                {
                    // ALL: product phải có tất cả optionIds
                    query = query.Where(p =>
                        p.ProductAttributeOptions
                         .Where(pa => optionIds.Contains(pa.AttributeOptionId))
                         .Select(pa => pa.AttributeOptionId)
                         .Distinct()
                         .Count() == optionIds.Count);
                }

                var totalCount = await query.CountAsync();

                int pageSize = size ?? 10;
                int pageNumber = page ?? 1;

                // Sort logic
                bool isDesc = sortOrder == SortOrder.Desc;
                switch (sortBy)
                {
                    case SortField.DiscountPrice:
                        query = isDesc
                            ? query.OrderByDescending(p => p.DiscountPrice)
                            : query.OrderBy(p => p.DiscountPrice);
                        break;

                    case SortField.DiscountPercent:
                        query = isDesc
                            ? query.OrderByDescending(p => p.DiscountPercent)
                            : query.OrderBy(p => p.DiscountPercent);
                        break;

                    default:
                        query = query.OrderByDescending(p => p.Id);
                        break;
                }

                var data = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    total = totalCount,
                    page = pageNumber,
                    size = pageSize,
                    sortBy,
                    sortOrder,
                    items = data
                });
            }
            catch (Exception ex)
            {
                // Ghi log lỗi ra console (bạn có thể dùng ILogger nếu có sẵn)
                Console.WriteLine("Lỗi khi lấy sản phẩm có filter: " + ex.Message);
                Console.WriteLine(ex.StackTrace);

                return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
            }
        }

        // Controller method


        // POST: api/products/bulk-update-gallery
        [HttpPost("bulk-update-gallery")]
        public async Task<IActionResult> BulkUpdateGallery([FromBody] List<UpdateProductGalleryDTO> items)
        {
            if (items == null || items.Count == 0)
                return BadRequest(new { message = "Danh sách rỗng" });

            var updated = new List<object>();
            var skipped = new List<object>();

            foreach (var dto in items)
            {
                // basic validation: cần id hoặc code
                if (dto.IdProduct == null && string.IsNullOrWhiteSpace(dto.CodeProduct))
                {
                    skipped.Add(new { dto.IdProduct, dto.CodeProduct, reason = "Thiếu idProduct và codeProduct" });
                    continue;
                }

                Product? product = null;

                try
                {
                    if (dto.IdProduct != null && dto.IdProduct > 0)
                    {
                        product = await _context.Products.FindAsync(dto.IdProduct.Value);
                    }

                    // nếu chưa tìm bằng id, thử tìm bằng code (case-insensitive)
                    if (product == null && !string.IsNullOrWhiteSpace(dto.CodeProduct))
                    {
                        var code = dto.CodeProduct.Trim();
                        product = await _context.Products
                                                .FirstOrDefaultAsync(p => p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
                    }

                    if (product == null)
                    {
                        skipped.Add(new { dto.IdProduct, dto.CodeProduct, reason = "Không tìm thấy sản phẩm" });
                        continue;
                    }

                    // cập nhật Gallery (nếu dto.Gallery null => coi như xóa/clear, tuỳ ý)
                    product.Gallery = dto.Gallery ?? string.Empty;

                    product.Image = dto.Image ?? string.Empty;

                    // nếu bạn muốn timestamp/updatedBy, cập nhật thêm ở đây
                    // product.UpdatedAt = DateTime.UtcNow;

                    updated.Add(new { product.Id, product.Code, product.Name, newGallery = product.Gallery, newImage = product.Image });

                    // mark entity modified is not required if tracked; but ensure tracked
                    _context.Products.Update(product);
                }
                catch (Exception ex)
                {
                    skipped.Add(new { dto.IdProduct, dto.CodeProduct, reason = "Lỗi khi xử lý", error = ex.Message });
                }
            }

            try
            {
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Hoàn tất cập nhật gallery",
                    updatedCount = updated.Count,
                    skippedCount = skipped.Count,
                    updated,
                    skipped
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lưu dữ liệu", error = ex.Message });
            }
        }



    }
}
