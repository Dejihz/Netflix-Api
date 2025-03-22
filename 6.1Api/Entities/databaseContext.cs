using Microsoft.EntityFrameworkCore;
using project6._1Api.Model;
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
            //modelBuilder.Entity<Transactions>()
            //    .ToTable(tb => tb.HasTrigger("CreateLog"));

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.RefreshToken).IsRequired(false); // Optional
                entity.Property(u => u.RefreshTokenExpiryTime).IsRequired(false); // Optional
            });
        }
    }
}
