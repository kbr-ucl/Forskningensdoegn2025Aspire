using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceA.Model;

namespace ServiceA.Controllers;

[Route("[controller]")] //Tilrettet
[ApiController]
public class ServiceAEntitiesController : ControllerBase
{
    private readonly ServiceADbContext _context;

    public ServiceAEntitiesController(ServiceADbContext context)
    {
        _context = context;
    }

    // GET: ServiceAEntities
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceAEntityDto>>> GetServiceAEntites()
    {
        return await _context.ServiceAEntites
            .Select(a => new ServiceAEntityDto(a.Id, a.Name, a.Description))
            .ToListAsync();
    }

    // GET: ServiceAEntities/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceAEntityDto>> GetServiceAEntity(int id)
    {
        var serviceAEntity = await _context.ServiceAEntites.FindAsync(id);

        if (serviceAEntity == null) return NotFound();

        return new ServiceAEntityDto(serviceAEntity.Id, serviceAEntity.Name, serviceAEntity.Description);
    }

    // PUT: ServiceAEntities/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutServiceAEntity(int id, ServiceAEntityDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var serviceAEntity = ConvertFromDto(dto);
        _context.Entry(serviceAEntity).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ServiceAEntityExists(id)) return NotFound();

            throw;
        }

        return NoContent();
    }

    // POST: ServiceAEntities
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ServiceAEntityDto>> PostServiceAEntity(ServiceAEntityDto dto)
    {
        var serviceAEntity = ConvertFromDto(dto);
        _context.ServiceAEntites.Add(serviceAEntity);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetServiceAEntity", new { id = serviceAEntity.Id },
            new ServiceAEntityDto(serviceAEntity.Id, serviceAEntity.Name, serviceAEntity.Description));
    }

    // DELETE: ServiceAEntities/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServiceAEntity(int id)
    {
        var serviceAEntity = await _context.ServiceAEntites.FindAsync(id);
        if (serviceAEntity == null) return NotFound();

        _context.ServiceAEntites.Remove(serviceAEntity);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ServiceAEntityExists(int id)
    {
        return _context.ServiceAEntites.Any(e => e.Id == id);
    }

    private ServiceAEntity ConvertFromDto(ServiceAEntityDto entity)
    {
        return new ServiceAEntity
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}

// DTO for ServiceAEntity
public record ServiceAEntityDto(int Id, string Name, string Description);