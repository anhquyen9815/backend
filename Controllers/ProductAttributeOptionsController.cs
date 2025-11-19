using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DienMayLongQuyen.Api.Data;
using DienMayLongQuyen.Api.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.Data.Sqlite;

namespace DienMayLongQuyen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductAttributeOptionsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProductAttributeOptionsController(AppDbContext db) => _db = db;

        // GET: api/ProductAttributeOptions
        // Query params: productId, attributeOptionId, attributeDefinitionId, categoryId, brandId, page, pageSize, sort
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? productId = null,
            [FromQuery] int? attributeOptionId = null,
            [FromQuery] int? attributeDefinitionId = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] int? brandId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string sort = "id"
            )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 500);

            var query = _db.ProductAttributeOptions
                .Include(pao => pao.AttributeOption)
                    .ThenInclude(ao => ao.AttributeDefinition)
                .Include(pao => pao.Product)
                .AsQueryable();

            // Filters
            if (productId.HasValue)
                query = query.Where(x => x.ProductId == productId.Value);

            if (attributeOptionId.HasValue)
                query = query.Where(x => x.AttributeOptionId == attributeOptionId.Value);

            // use FK on AttributeOption for reliable SQL translation
            if (attributeDefinitionId.HasValue)
                query = query.Where(x => x.AttributeOption.AttributeDefinitionId == attributeDefinitionId.Value);

            if (categoryId.HasValue)
                query = query.Where(x => x.Product.CategoryId == categoryId.Value);

            if (brandId.HasValue)
                query = query.Where(x => x.Product.BrandId == brandId.Value);

            // Sorting (null-safe / consistent with secondary ordering)
            query = sort?.ToLowerInvariant() switch
            {
                "product" => query.OrderBy(x => x.ProductId).ThenBy(x => x.Id),
                "product_desc" => query.OrderByDescending(x => x.ProductId).ThenByDescending(x => x.Id),
                "option" => query.OrderBy(x => x.AttributeOptionId).ThenBy(x => x.Id),
                "option_desc" => query.OrderByDescending(x => x.AttributeOptionId).ThenByDescending(x => x.Id),
                "definition" => query.OrderBy(x => x.AttributeOption.AttributeDefinitionId).ThenBy(x => x.Id),
                "definition_desc" => query.OrderByDescending(x => x.AttributeOption.AttributeDefinitionId).ThenByDescending(x => x.Id),
                "category" => query.OrderBy(x => x.Product.CategoryId).ThenBy(x => x.Id),
                "category_desc" => query.OrderByDescending(x => x.Product.CategoryId).ThenByDescending(x => x.Id),
                "brand" => query.OrderBy(x => x.Product.BrandId).ThenBy(x => x.Id),
                "brand_desc" => query.OrderByDescending(x => x.Product.BrandId).ThenByDescending(x => x.Id),
                _ => query.OrderBy(x => x.Id)
            };

            // DEV: bỏ comment để in SQL và kiểm tra (chỉ dev)
            // Console.WriteLine(query.ToQueryString());

            var total = await query.LongCountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ProductAttributeOptionDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    AttributeOptionId = x.AttributeOptionId,
                    OptionLabel = x.AttributeOption != null ? x.AttributeOption.Label : null,
                    OptionValueKey = x.AttributeOption != null ? x.AttributeOption.ValueKey : null,
                    AttributeName = x.AttributeOption != null && x.AttributeOption.AttributeDefinition != null
                        ? x.AttributeOption.AttributeDefinition.Name
                        : null
                })
                .ToListAsync();

            return Ok(new
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = items
            });
        }




        // GET: api/ProductAttributeOptions/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pao = await _db.ProductAttributeOptions
                .Include(x => x.AttributeOption).ThenInclude(ao => ao.AttributeDefinition)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pao == null) return NotFound();

            var dto = new ProductAttributeOptionDto
            {
                Id = pao.Id,
                ProductId = pao.ProductId,
                AttributeOptionId = pao.AttributeOptionId,
                OptionLabel = pao.AttributeOption?.Label,
                OptionValueKey = pao.AttributeOption?.ValueKey,
                AttributeName = pao.AttributeOption?.AttributeDefinition?.Name
            };

            return Ok(dto);
        }

        // POST: api/ProductAttributeOptions
        // Create a link between product and option
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductAttributeOptionCreateDto input)
        {
            if (input == null) return BadRequest("Payload required.");
            if (input.ProductId <= 0) return BadRequest("ProductId is required.");
            if (input.AttributeOptionId <= 0) return BadRequest("AttributeOptionId is required.");

            // validate product
            var product = await _db.Products.FindAsync(input.ProductId);
            if (product == null) return BadRequest($"Product {input.ProductId} not found.");

            // validate attribute option and load definition
            var option = await _db.AttributeOptions
                .Include(o => o.AttributeDefinition)
                .FirstOrDefaultAsync(o => o.Id == input.AttributeOptionId);

            if (option == null) return BadRequest($"AttributeOption {input.AttributeOptionId} not found.");

            // ensure category match (prevent cross-category link)
            if (option.AttributeDefinition != null && option.AttributeDefinition.CategoryId != product.CategoryId)
            {
                return Conflict(new { message = "AttributeOption's attribute does not belong to the same Category as the Product." });
            }

            // prevent duplicate
            var exists = await _db.ProductAttributeOptions
                .AnyAsync(pao => pao.ProductId == input.ProductId && pao.AttributeOptionId == input.AttributeOptionId);
            if (exists)
            {
                return Conflict(new { message = "This product already has this attribute option assigned." });
            }

            var entity = new ProductAttributeOption
            {
                ProductId = input.ProductId,
                AttributeOptionId = input.AttributeOptionId
            };

            _db.ProductAttributeOptions.Add(entity);
            await _db.SaveChangesAsync();

            var dto = new ProductAttributeOptionDto
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                AttributeOptionId = entity.AttributeOptionId,
                OptionLabel = option.Label,
                OptionValueKey = option.ValueKey,
                AttributeName = option.AttributeDefinition?.Name
            };

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        // PUT: api/ProductAttributeOptions/{id}
        // Update: change option assigned to the product (less common)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductAttributeOptionUpdateDto input)
        {
            if (input == null) return BadRequest("Payload required.");

            var entity = await _db.ProductAttributeOptions
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) return NotFound();

            // treat 0 or negative as "not provided"
            if (input.AttributeOptionId > 0)
            {
                var newOptionId = input.AttributeOptionId;
                var newOption = await _db.AttributeOptions
                    .Include(o => o.AttributeDefinition)
                    .FirstOrDefaultAsync(o => o.Id == newOptionId);

                if (newOption == null) return BadRequest($"AttributeOption {newOptionId} not found.");

                if (entity.Product == null)
                {
                    entity.Product = await _db.Products.FindAsync(entity.ProductId);
                    if (entity.Product == null) return BadRequest($"Product {entity.ProductId} not found.");
                }

                if (newOption.AttributeDefinition != null && newOption.AttributeDefinition.CategoryId != entity.Product.CategoryId)
                {
                    return Conflict(new { message = "New option's attribute does not belong to the same Category as the Product." });
                }

                var dup = await _db.ProductAttributeOptions
                    .AnyAsync(pao => pao.ProductId == entity.ProductId && pao.AttributeOptionId == newOptionId && pao.Id != id);
                if (dup) return Conflict(new { message = "Product already has that option assigned." });

                entity.AttributeOptionId = newOptionId;
            }

            _db.ProductAttributeOptions.Update(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/ProductAttributeOptions/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.ProductAttributeOptions.FindAsync(id);
            if (entity == null) return NotFound();

            _db.ProductAttributeOptions.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/ProductAttributeOptions/bulk?productId=1&replace=true
        // Bulk assign many optionIds for a product (optionally replace existing)
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkAssign(
            [FromQuery] int productId,
            [FromQuery] bool replace = false,
            [FromBody] int[] optionIds = null)
        {
            if (productId <= 0) return BadRequest("productId required.");
            if (optionIds == null) optionIds = Array.Empty<int>();

            var product = await _db.Products.FindAsync(productId);
            if (product == null) return BadRequest($"Product {productId} not found.");

            // load options with definition
            var options = await _db.AttributeOptions
                .Include(o => o.AttributeDefinition)
                .Where(o => optionIds.Contains(o.Id))
                .ToListAsync();

            // ensure all option ids valid
            var missing = optionIds.Except(options.Select(o => o.Id)).ToList();
            if (missing.Any()) return BadRequest(new { message = "Some AttributeOption ids not found", missing });

            // ensure category matching
            var mismatches = options
                .Where(o => o.AttributeDefinition != null && o.AttributeDefinition.CategoryId != product.CategoryId)
                .Select(o => o.Id)
                .ToList();
            if (mismatches.Any()) return Conflict(new { message = "Some options do not belong to product's category", mismatches });

            // Transactional block to make replace/remove + insert atomic
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                if (replace)
                {
                    var existingAll = _db.ProductAttributeOptions.Where(pao => pao.ProductId == productId);
                    _db.ProductAttributeOptions.RemoveRange(existingAll);
                    await _db.SaveChangesAsync();
                }

                // Get existing optionIds for this product (after possible removal)
                var existingOptionIds = await _db.ProductAttributeOptions
                    .Where(pao => pao.ProductId == productId && optionIds.Contains(pao.AttributeOptionId))
                    .Select(pao => pao.AttributeOptionId)
                    .ToListAsync();

                // compute toInsert (exclude existing)
                var toInsertIds = optionIds.Except(existingOptionIds).Distinct().ToList();

                var toInsert = toInsertIds.Select(optId => new ProductAttributeOption
                {
                    ProductId = productId,
                    AttributeOptionId = optId
                }).ToList();

                var added = new List<int>();
                var already = existingOptionIds;

                if (toInsert.Any())
                {
                    _db.ProductAttributeOptions.AddRange(toInsert);
                    try
                    {
                        await _db.SaveChangesAsync();
                        added.AddRange(toInsert.Select(x => x.AttributeOptionId));
                    }
                    catch (DbUpdateException dbEx)
                    {
                        if (IsUniqueConstraintViolation(dbEx))
                        {
                            // Race: some inserts already created by other request.
                            // Option: return 409 or fallback to success with details. We'll return 409 with message + partial info.
                            await tx.RollbackAsync();
                            return Conflict(new { message = "One or more assignments already exist (unique constraint)." });
                        }
                        throw;
                    }
                }
                await tx.CommitAsync();

                return Ok(new
                {
                    added,
                    already,
                    missing,
                    mismatches
                });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return StatusCode(500, new { message = "Error during bulk assign", detail = ex.Message });
            }
        }


        /// <summary>
        /// POST /api/ProductAttributeOptions/assignOptionToProducts?optionId=45&replaceSameAttribute=true
        /// Body: [1,2,3]  (product ids)
        /// - optionId: the AttributeOption to assign
        /// - replaceSameAttribute: if true, remove other ProductAttributeOptions on those products that belong to the same AttributeDefinition as the provided option
        /// Returns details: added, already (skipped), missingProducts, mismatches (category)
        /// </summary>
        [HttpPost("assignOptionToProducts")]
        public async Task<IActionResult> AssignOptionToProducts(
            [FromBody] AssignOptionToProductsRequest request
            )
        {

            if (request == null) return BadRequest("Request body is required.");
            if (request.ProductIds == null || request.ProductIds.Length == 0)
                return BadRequest("ProductIds is required.");

            int optionId = request.OptionId;
            bool replaceSameAttribute = request.ReplaceSameAttribute;
            int[] productIds = request.ProductIds;

            if (optionId <= 0) return BadRequest("optionId required.");
            if (productIds == null || productIds.Length == 0)
                return BadRequest("productIds array required in body.");

            var option = await _db.AttributeOptions
                .Include(o => o.AttributeDefinition)
                .FirstOrDefaultAsync(o => o.Id == optionId);

            if (option == null) return BadRequest($"AttributeOption {optionId} not found.");

            var attrDefId = option.AttributeDefinition?.Id;
            var optionAttrCategoryId = option.AttributeDefinition?.CategoryId;

            var products = await _db.Products
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new { p.Id, p.CategoryId })
                .ToListAsync();

            var foundProductIds = products.Select(p => p.Id).ToHashSet();
            var missingProducts = productIds.Except(foundProductIds).ToList();

            var mismatches = new List<int>();
            if (optionAttrCategoryId.HasValue)
            {
                mismatches = products
                    .Where(p => p.CategoryId != optionAttrCategoryId.Value)
                    .Select(p => p.Id)
                    .ToList();
            }

            if (mismatches.Any())
            {
                return Conflict(new { message = "Some products' category do not match the option's attribute category.", mismatches, missingProducts });
            }

            var added = new List<int>();
            var already = new List<int>();

            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                if (replaceSameAttribute && attrDefId.HasValue)
                {
                    var toRemove = _db.ProductAttributeOptions
                        .Where(pao => productIds.Contains(pao.ProductId))
                        .Join(_db.AttributeOptions,
                              pao => pao.AttributeOptionId,
                              ao => ao.Id,
                              (pao, ao) => new { pao, ao })
                        .Where(joined => joined.ao.AttributeDefinitionId == attrDefId.Value)
                        .Select(joined => joined.pao);

                    _db.ProductAttributeOptions.RemoveRange(toRemove);
                    await _db.SaveChangesAsync();
                }

                // existing links for this optionId
                var existingLinks = await _db.ProductAttributeOptions
                    .Where(pao => productIds.Contains(pao.ProductId) && pao.AttributeOptionId == optionId)
                    .Select(pao => pao.ProductId)
                    .ToListAsync();

                var existingSet = existingLinks.ToHashSet();

                // create only for productIds not in existingSet, not missing, not mismatched
                var toInsertProductIds = productIds
                    .Where(pid => !existingSet.Contains(pid) && !missingProducts.Contains(pid) && !mismatches.Contains(pid))
                    .Distinct()
                    .ToList();

                var toInsert = toInsertProductIds.Select(pid => new ProductAttributeOption { ProductId = pid, AttributeOptionId = optionId }).ToList();

                if (toInsert.Any())
                {
                    _db.ProductAttributeOptions.AddRange(toInsert);
                    try
                    {
                        await _db.SaveChangesAsync();
                        added.AddRange(toInsert.Select(x => x.ProductId));
                    }
                    catch (DbUpdateException dbEx)
                    {
                        if (IsUniqueConstraintViolation(dbEx))
                        {
                            await tx.RollbackAsync();
                            return Conflict(new { message = "One or more assignments already exist (unique constraint)." });
                        }
                        throw;
                    }
                }

                already.AddRange(existingSet);
                await tx.CommitAsync();
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return StatusCode(500, new { message = "An error occurred while assigning option to products.", detail = ex.Message });
            }

            return Ok(new { added, already, missingProducts, mismatches });
        }



        private bool IsUniqueConstraintViolation(DbUpdateException dbEx)
        {
            var inner = dbEx.InnerException;
            if (inner == null) return false;

            // SQLite (SqliteException.SqliteErrorCode == 19 -> constraint failed)
            if (inner is SqliteException sqliteEx)
            {
                return sqliteEx.SqliteErrorCode == 19
                       || (sqliteEx.Message?.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) ?? false);
            }

            // // SQL Server (SqlException.Number: 2627 = PK/unique, 2601 = unique index)
            // if (inner is SqlException sqlEx)
            // {
            //     return sqlEx.Number == 2627 || sqlEx.Number == 2601
            //            || (sqlEx.Message?.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) ?? false);
            // }

            // // PostgreSQL (unique violation SQLSTATE = 23505)
            // if (inner is PostgresException pgEx)
            // {
            //     return pgEx.SqlState == "23505"
            //            || (pgEx.Message?.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) ?? false);
            // }

            // Fallback: check message text for UNIQUE / duplicate
            var text = inner.Message ?? string.Empty;
            if (text.IndexOf("unique", StringComparison.OrdinalIgnoreCase) >= 0
                || text.IndexOf("duplicate", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            return false;
        }
    }
}
