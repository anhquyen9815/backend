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
    public class ProductWarrantiesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProductWarrantiesController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/productwarranties
        // filters: productId, warrantyId, isDeleted, page, pageSize
        [HttpGet]
        public async Task<IActionResult> GetList(
            [FromQuery] int? productId,
            [FromQuery] int? warrantyId,
            [FromQuery] int? categoryId,
            [FromQuery] int? brandId,
            [FromQuery] bool? isDeleted,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;

            var q = _db.ProductWarranties
                .Include(pw => pw.Warranty)
                .Include(pw => pw.Product)
                .AsQueryable();

            if (productId.HasValue) q = q.Where(pw => pw.ProductId == productId.Value);
            if (warrantyId.HasValue) q = q.Where(pw => pw.WarrantyId == warrantyId.Value);
            // lọc theo category / brand (qua navigation Product)
            if (categoryId.HasValue) q = q.Where(pw => pw.Product.CategoryId == categoryId.Value);
            if (brandId.HasValue) q = q.Where(pw => pw.Product.BrandId == brandId.Value);

            if (isDeleted.HasValue) q = q.Where(pw => pw.IsDeleted == isDeleted.Value);
            else q = q.Where(pw => pw.IsDeleted == false);

            var total = await q.CountAsync();
            var items = await q
                .OrderByDescending(pw => pw.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new { total, page, pageSize, items });
        }

        // GET: api/productwarranties/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var pw = await _db.ProductWarranties
                .Include(p => p.Warranty)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pw == null) return NotFound();
            return Ok(pw);
        }

        // POST: api/productwarranties
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductWarrantyDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Basic validation: product & warranty exist
            var product = await _db.Products.FindAsync(dto.ProductId);
            if (product == null) return BadRequest($"Product {dto.ProductId} not found.");

            var warranty = await _db.Warranties.FindAsync(dto.WarrantyId);
            if (warranty == null) return BadRequest($"Warranty {dto.WarrantyId} not found.");

            // Check unique constraint (ProductId + WarrantyId)
            var exists = await _db.ProductWarranties.AnyAsync(x =>
                x.ProductId == dto.ProductId && x.WarrantyId == dto.WarrantyId && !x.IsDeleted);
            if (exists) return Conflict("This product already has the same warranty assigned.");

            var now = DateTime.UtcNow;
            var pw = new ProductWarranty
            {
                ProductId = dto.ProductId,
                WarrantyId = dto.WarrantyId,
                WarrantyNumber = dto.WarrantyNumber,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                PeriodMonths = dto.PeriodMonths,
                Price = dto.Price,
                Notes = dto.Notes,
                IsDeleted = false,
                CreatedAt = now,
                UpdatedAt = now
            };

            _db.ProductWarranties.Add(pw);
            await _db.SaveChangesAsync();

            // reload include warranty
            await _db.Entry(pw).Reference(x => x.Warranty).LoadAsync();

            return CreatedAtAction(nameof(Get), new { id = pw.Id }, pw);
        }

        // POST: api/productwarranties/bulk
        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBulk([FromBody] List<CreateProductWarrantyDTO> list)
        {
            if (list == null || list.Count == 0) return BadRequest("Empty payload");

            var productIds = list.Select(x => x.ProductId).Distinct().ToList();
            var warrantyIds = list.Select(x => x.WarrantyId).Distinct().ToList();

            // validate existence
            var productsExist = await _db.Products.Where(p => productIds.Contains(p.Id)).Select(p => p.Id).ToListAsync();
            var warrantiesExist = await _db.Warranties.Where(w => warrantyIds.Contains(w.Id)).Select(w => w.Id).ToListAsync();

            var notFoundProduct = productIds.Except(productsExist).ToList();
            if (notFoundProduct.Any()) return BadRequest($"Products not found: {string.Join(',', notFoundProduct)}");

            var notFoundWarranty = warrantyIds.Except(warrantiesExist).ToList();
            if (notFoundWarranty.Any()) return BadRequest($"Warranties not found: {string.Join(',', notFoundWarranty)}");

            // filter out duplicates within DB for same product+warranty
            var existingPairs = await _db.ProductWarranties
                .Where(pw => !pw.IsDeleted)
                .Select(pw => new { pw.ProductId, pw.WarrantyId })
                .ToListAsync();

            var now = DateTime.UtcNow;
            var toInsert = new List<ProductWarranty>();
            foreach (var dto in list)
            {
                if (existingPairs.Any(e => e.ProductId == dto.ProductId && e.WarrantyId == dto.WarrantyId))
                    continue; // skip duplicates

                var pw = new ProductWarranty
                {
                    ProductId = dto.ProductId,
                    WarrantyId = dto.WarrantyId,
                    WarrantyNumber = dto.WarrantyNumber,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    PeriodMonths = dto.PeriodMonths,
                    Price = dto.Price,
                    Notes = dto.Notes,
                    IsDeleted = false,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                toInsert.Add(pw);
            }

            if (toInsert.Count == 0) return Conflict("No new entries to insert (duplicates skipped).");

            _db.ProductWarranties.AddRange(toInsert);
            await _db.SaveChangesAsync();

            return Ok(new { created = toInsert.Count });
        }

        // PUT: api/productwarranties/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductWarrantyDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pw = await _db.ProductWarranties.FindAsync(id);
            if (pw == null) return NotFound();

            // If changing productId or warrantyId, ensure no duplicate pair exists
            if ((dto.ProductId != 0 && dto.ProductId != pw.ProductId)
    || (dto.WarrantyId != 0 && dto.WarrantyId != pw.WarrantyId))
            {
                var newProductId = dto.ProductId;
                var newWarrantyId = dto.WarrantyId;

                var exists = await _db.ProductWarranties.AnyAsync(x =>
                    x.Id != id && x.ProductId == newProductId && x.WarrantyId == newWarrantyId && !x.IsDeleted);
                if (exists) return Conflict("Another product-warranty with same productId and warrantyId already exists.");
            }

            // apply updates
            pw.ProductId = dto.ProductId;
            pw.WarrantyId = dto.WarrantyId;
            pw.WarrantyNumber = dto.WarrantyNumber ?? pw.WarrantyNumber;
            pw.StartDate = dto.StartDate ?? pw.StartDate;
            pw.EndDate = dto.EndDate ?? pw.EndDate;
            pw.PeriodMonths = dto.PeriodMonths ?? pw.PeriodMonths;
            pw.Price = dto.Price ?? pw.Price;
            pw.Notes = dto.Notes ?? pw.Notes;
            pw.IsDeleted = dto.IsDeleted ?? pw.IsDeleted;
            pw.UpdatedAt = DateTime.UtcNow;

            _db.ProductWarranties.Update(pw);
            await _db.SaveChangesAsync();

            return Ok(pw);
        }

        // DELETE: api/productwarranties/{id}
        // soft delete
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool hard = false)
        {
            var pw = await _db.ProductWarranties.FindAsync(id);
            if (pw == null) return NotFound();

            if (hard)
            {
                _db.ProductWarranties.Remove(pw);
            }
            else
            {
                pw.IsDeleted = true;
                pw.UpdatedAt = DateTime.UtcNow;
                _db.ProductWarranties.Update(pw);
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }


    }
}
