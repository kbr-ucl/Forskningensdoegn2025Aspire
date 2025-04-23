using Microsoft.EntityFrameworkCore;

namespace ServiceB.Model;

public class ServiceBDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ServiceBEntity> ServiceBEntites { get; set; }
}

public class ServiceBEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}