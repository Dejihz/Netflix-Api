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
        public DbSet<Subscriptions> Subscription { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Transactions>()
            //    .ToTable(tb => tb.HasTrigger("CreateLog"));

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("trg_DefaultUserRole"));
                entity.ToTable(tb => tb.HasTrigger("trg_UserRoleChange"));
                entity.ToTable(tb => tb.HasTrigger("trg_PreventDuplicateEmails"));
            });

        }
    }
}
