using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceB.Model;

namespace ServiceB.Controllers;

[Route("[controller]")]
[ApiController]
public class ServiceBEntitiesController : ControllerBase
{
    private readonly ServiceBDbContext _context;

    public ServiceBEntitiesController(ServiceBDbContext context)
    {
        _context = context;
    }

    // GET: ServiceBEntities
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceBEntityDto>>> GetServiceBEntites()
    {
        return await _context.ServiceBEntites
            .Select(a => new ServiceBEntityDto(a.Id, a.Name, a.Description))
            .ToListAsync();
    }

    // GET: ServiceBEntities/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceBEntityDto>> GetServiceBEntity(int id)
    {
        var serviceAEntity = await _context.ServiceBEntites.FindAsync(id);

        if (serviceAEntity == null) return NotFound();

        return new ServiceBEntityDto(serviceAEntity.Id, serviceAEntity.Name, serviceAEntity.Description);
    }

    // PUT: ServiceBEntities/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutServiceBEntity(int id, ServiceBEntityDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var serviceAEntity = ConvertFromDto(dto);
        _context.Entry(serviceAEntity).State = EntityState.Modified;
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

    // POST: ServiceBEntities
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ServiceBEntityDto>> PostServiceBEntity(ServiceBEntityDto dto)
    {
        var serviceAEntity = ConvertFromDto(dto);
        _context.ServiceBEntites.Add(serviceAEntity);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetServiceBEntity", new { id = serviceAEntity.Id },
            new ServiceBEntityDto(serviceAEntity.Id, serviceAEntity.Name, serviceAEntity.Description));
    }

    // DELETE: ServiceBEntities/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServiceBEntity(int id)
    {
        var serviceAEntity = await _context.ServiceBEntites.FindAsync(id);
        if (serviceAEntity == null) return NotFound();

        _context.ServiceBEntites.Remove(serviceAEntity);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ServiceAEntityExists(int id)
    {
        return _context.ServiceBEntites.Any(e => e.Id == id);
    }

    private ServiceBEntity ConvertFromDto(ServiceBEntityDto entity)
    {
        return new ServiceBEntity
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}

// DTO for ServiceBEntity
public record ServiceBEntityDto(int Id, string Name, string Description);