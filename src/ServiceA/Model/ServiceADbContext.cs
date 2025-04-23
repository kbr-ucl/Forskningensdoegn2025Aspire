using Microsoft.EntityFrameworkCore;

namespace ServiceA.Model
{
    public class ServiceADbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<ServiceAEntity> ServiceAEntites { get; set; }
    }

    public class ServiceAEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
