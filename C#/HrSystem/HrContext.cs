using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HrSystem
{
    public class HrContext : DbContext
    {
        private IConfiguration _configuration;

        public DbSet<Employee> Employees { get; set; }

        public HrContext(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlDatabase"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Employee>().ToTable("Employees", "hr");
        }
    }
}