using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DienMayLongQuyen.Api.Data;
using DienMayLongQuyen.Api.Models;

[ApiController]
[Route("api/[controller]")]
public class AttributeOptionsController : ControllerBase
{
    private readonly AppDbContext _db;
    public AttributeOptionsController(AppDbContext db) => _db = db;

    // GET: api/AttributeOptions
    // Query params:
    // - attributeDefinitionId (optional) => get options only for that attribute
    // - search (ValueKey or Label)
    // - page, pageSize
    // - sort (displayOrder|valuekey|label)
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? attributeDefinitionId = null,
        [FromQuery] string search = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 100,
        [FromQuery] string sort = "displayOrder")
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 1000);

        var query = _db.AttributeOptions.AsQueryable();

        if (attributeDefinitionId.HasValue)
            query = query.Where(o => o.AttributeDefinitionId == attributeDefinitionId.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(o => o.ValueKey.Contains(s) || o.Label.Contains(s));
        }

        query = sort?.ToLowerInvariant() switch
        {
            "valuekey" => query.OrderBy(o => o.ValueKey),
            "valuekey_desc" => query.OrderByDescending(o => o.ValueKey),
            "label" => query.OrderBy(o => o.Label),
            "label_desc" => query.OrderByDescending(o => o.Label),
            "displayorder_desc" => query.OrderByDescending(o => o.DisplayOrder),
            _ => query.OrderBy(o => o.DisplayOrder).ThenBy(o => o.Label),
        };

        var total = await query.LongCountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new AttributeOptionDto
            {
                Id = o.Id,
                AttributeDefinitionId = o.AttributeDefinitionId,
                ValueKey = o.ValueKey,
                Label = o.Label,
                DisplayOrder = o.DisplayOrder
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

    // GET: api/AttributeOptions/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var opt = await _db.AttributeOptions.FindAsync(id);
        if (opt == null) return NotFound();

        var dto = new AttributeOptionDto
        {
            Id = opt.Id,
            AttributeDefinitionId = opt.AttributeDefinitionId,
            ValueKey = opt.ValueKey,
            Label = opt.Label,
            DisplayOrder = opt.DisplayOrder
        };
        return Ok(dto);
    }

    // POST: api/AttributeOptions
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AttributeOptionCreateDto input)
    {
        if (input == null) return BadRequest("Payload required.");
        if (input.AttributeDefinitionId <= 0) return BadRequest("AttributeDefinitionId is required.");
        if (string.IsNullOrWhiteSpace(input.ValueKey)) return BadRequest("ValueKey is required.");
        if (string.IsNullOrWhiteSpace(input.Label)) return BadRequest("Label is required.");

        // ensure attribute definition exists
        var attrDef = await _db.AttributeDefinitions.FindAsync(input.AttributeDefinitionId);
        if (attrDef == null) return BadRequest($"AttributeDefinition {input.AttributeDefinitionId} not found.");

        var key = input.ValueKey.Trim();

        // prevent duplicate ValueKey in same attribute
        var exists = await _db.AttributeOptions
            .AnyAsync(o => o.AttributeDefinitionId == input.AttributeDefinitionId && o.ValueKey == key);
        if (exists)
            return Conflict(new { message = "AttributeOption with same ValueKey already exists for this AttributeDefinition." });

        var entity = new AttributeOption
        {
            AttributeDefinitionId = input.AttributeDefinitionId,
            ValueKey = key,
            Label = input.Label.Trim(),
            DisplayOrder = input.DisplayOrder
        };

        _db.AttributeOptions.Add(entity);
        await _db.SaveChangesAsync();

        var dto = new AttributeOptionDto
        {
            Id = entity.Id,
            AttributeDefinitionId = entity.AttributeDefinitionId,
            ValueKey = entity.ValueKey,
            Label = entity.Label,
            DisplayOrder = entity.DisplayOrder
        };

        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    // PUT: api/AttributeOptions/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AttributeOptionUpdateDto input)
    {
        if (input == null) return BadRequest("Payload required.");

        var entity = await _db.AttributeOptions.FindAsync(id);
        if (entity == null) return NotFound();

        // If ValueKey is updated, check duplicate in the same attributeDefinition
        if (!string.IsNullOrWhiteSpace(input.ValueKey))
        {
            var newKey = input.ValueKey.Trim();
            if (newKey != entity.ValueKey)
            {
                var dup = await _db.AttributeOptions
                    .AnyAsync(o => o.AttributeDefinitionId == entity.AttributeDefinitionId && o.ValueKey == newKey && o.Id != id);
                if (dup)
                    return Conflict(new { message = "Another option with same ValueKey exists for this attribute." });

                entity.ValueKey = newKey;
            }
        }

        if (input.Label != null)
            entity.Label = input.Label.Trim();

        if (input.DisplayOrder.HasValue)
            entity.DisplayOrder = input.DisplayOrder.Value;

        _db.AttributeOptions.Update(entity);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/AttributeOptions/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.AttributeOptions.FindAsync(id);
        if (entity == null) return NotFound();

        // Prevent delete if product options reference it
        var inUse = await _db.ProductAttributeOptions.AnyAsync(pao => pao.AttributeOptionId == id);
        if (inUse)
        {
            return Conflict(new
            {
                message = "Cannot delete attribute option because some products reference it. Remove those links first."
            });
        }

        _db.AttributeOptions.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
