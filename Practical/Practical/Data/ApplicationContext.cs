using Microsoft.EntityFrameworkCore;
using Practical.Models;

namespace Practical.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
        }
        public DbSet<Employee> Employee { get; set; }

        public DbSet<Department> Department { get; set; }
    }
}
