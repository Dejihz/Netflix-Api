using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace project6._1Api.Entities
{
    public class databaseContext : DbContext
    {
        public databaseContext(DbContextOptions<databaseContext> options)
            : base(options)
        {
        }

        public DbSet<Transactions> Transaction { get; set; }
        public DbSet<Users> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transactions>()
                .ToTable(tb => tb.HasTrigger("CreateLog"));
        }
    }
}
