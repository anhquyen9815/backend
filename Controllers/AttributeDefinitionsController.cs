using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DienMayLongQuyen.Api.Data;
using DienMayLongQuyen.Api.Models;

[ApiController]
[Route("api/[controller]")]
public class AttributeDefinitionsController : ControllerBase
{
    private readonly AppDbContext _db;

    public AttributeDefinitionsController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/AttributeDefinitions
    // Optional query: categoryId, search (on DisplayName or Name), page, pageSize, sort (name|displayOrder)
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? categoryId,
        [FromQuery] string search = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string sort = "displayOrder")
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 200);

        var query = _db.AttributeDefinitions.AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(ad => ad.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(ad => ad.Name.Contains(s) || ad.DisplayName.Contains(s));
        }

        // sorting
        query = sort?.ToLowerInvariant() switch
        {
            "name" => query.OrderBy(ad => ad.Name),
            "name_desc" => query.OrderByDescending(ad => ad.Name),
            "displayorder_desc" => query.OrderByDescending(ad => ad.DisplayOrder),
            _ => query.OrderBy(ad => ad.DisplayOrder).ThenBy(ad => ad.Name),
        };

        var total = await query.LongCountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ad => new AttributeDefinitionDto
            {
                Id = ad.Id,
                Name = ad.Name,
                DisplayName = ad.DisplayName,
                DataType = ad.DataType,
                DisplayOrder = ad.DisplayOrder,
                CategoryId = ad.CategoryId
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

    // GET: api/AttributeDefinitions/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ad = await _db.AttributeDefinitions.FindAsync(id);
        if (ad == null) return NotFound();

        var dto = new AttributeDefinitionDto
        {
            Id = ad.Id,
            Name = ad.Name,
            DisplayName = ad.DisplayName,
            DataType = ad.DataType,
            DisplayOrder = ad.DisplayOrder,
            CategoryId = ad.CategoryId
        };
        return Ok(dto);
    }

    // POST: api/AttributeDefinitions
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AttributeDefinitionCreateDto input)
    {
        if (input == null) return BadRequest("Payload required.");
        if (string.IsNullOrWhiteSpace(input.Name)) return BadRequest("Name is required.");
        if (string.IsNullOrWhiteSpace(input.DisplayName)) return BadRequest("DisplayName is required.");
        if (input.CategoryId <= 0) return BadRequest("CategoryId is required.");

        // normalize name
        var name = input.Name.Trim();

        // check category exists
        var catExists = await _db.Categories.AnyAsync(c => c.Id == input.CategoryId);
        if (!catExists) return BadRequest($"Category '{input.CategoryId}' not found.");

        // check duplicate Name in same category
        var dup = await _db.AttributeDefinitions
            .AnyAsync(a => a.CategoryId == input.CategoryId && a.Name == name);
        if (dup) return Conflict(new { message = "AttributeDefinition with same Name already exists in this category." });

        var entity = new AttributeDefinition
        {
            Name = name,
            DisplayName = input.DisplayName.Trim(),
            DataType = input.DataType?.Trim() ?? "String",
            DisplayOrder = input.DisplayOrder,
            CategoryId = input.CategoryId
        };

        _db.AttributeDefinitions.Add(entity);
        await _db.SaveChangesAsync();

        var dto = new AttributeDefinitionDto
        {
            Id = entity.Id,
            Name = entity.Name,
            DisplayName = entity.DisplayName,
            DataType = entity.DataType,
            DisplayOrder = entity.DisplayOrder,
            CategoryId = entity.CategoryId
        };

        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    // PUT: api/AttributeDefinitions/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AttributeDefinitionUpdateDto input)
    {
        if (input == null) return BadRequest("Payload required.");

        var entity = await _db.AttributeDefinitions.FindAsync(id);
        if (entity == null) return NotFound();

        if (input.DisplayName != null)
            entity.DisplayName = input.DisplayName.Trim();

        if (input.DataType != null)
            entity.DataType = input.DataType.Trim();

        if (input.DisplayOrder.HasValue)
            entity.DisplayOrder = input.DisplayOrder.Value;

        if (input.CategoryId.HasValue && input.CategoryId.Value != entity.CategoryId)
        {
            // ensure new category exists
            var catExists = await _db.Categories.AnyAsync(c => c.Id == input.CategoryId.Value);
            if (!catExists) return BadRequest($"Category '{input.CategoryId.Value}' not found.");

            // prevent changing category if there are existing options/values referencing this attribute
            var hasOptionsOrValues = await _db.AttributeOptions.AnyAsync(o => o.AttributeDefinitionId == id)
                                   || await _db.ProductAttributeValues.AnyAsync(v => v.AttributeDefinitionId == id);

            if (hasOptionsOrValues)
                return Conflict(new { message = "Cannot change CategoryId when attribute has options/values. Remove them first." });

            entity.CategoryId = input.CategoryId.Value;
        }

        // check duplicate name? Only if name change is allowed. We don't allow name change here.
        // If you want to allow Name update, implement similar duplicate check.

        _db.AttributeDefinitions.Update(entity);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/AttributeDefinitions/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.AttributeDefinitions.FindAsync(id);
        if (entity == null) return NotFound();

        // don't allow delete if referenced by options or product values
        var inUseOption = await _db.AttributeOptions.AnyAsync(o => o.AttributeDefinitionId == id);
        var inUseValues = await _db.ProductAttributeValues.AnyAsync(v => v.AttributeDefinitionId == id);
        if (inUseOption || inUseValues)
        {
            return Conflict(new
            {
                message = "Can't delete attribute because there are options or product values referencing it. Remove them first."
            });
        }

        _db.AttributeDefinitions.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
