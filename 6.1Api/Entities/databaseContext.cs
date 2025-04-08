    using Microsoft.EntityFrameworkCore;
using project6._1Api.Model;
using System.Data;
using System.Reflection.Metadata;

namespace project6._1Api.Entities
{
    public class databaseContext : DbContext
    {
        public databaseContext(DbContextOptions<databaseContext> options)
            : base(options)
        {
        }

        public DbSet<Users> User { get; set; }
        public DbSet<Subscriptions> Subscription { get; set; }
        public DbSet<Roles> Role { get; set; }
        public DbSet<Referrals> Referral { get; set; }
        public  DbSet<Profiles> Profile { get; set; }
        public  DbSet<Preferences> Preferences { get; set; }
        public DbSet<Profile_Genre> Profile_Genre { get; set; }
        public DbSet<Contents> Content { get; set; }
        public DbSet<Content_Genre> Content_Genre { get; set; }
        public DbSet<Films> Film { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Episodes> Episode { get; set; }
        public DbSet<Genres> Genre { get; set; }
        public DbSet<WatchLists> Watch_List { get; set; }
        public DbSet<WatchHistories> Watch_History { get; set; }
        public DbSet<Subtitles> Subtitles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Transactions>()
            //    .ToTable(tb => tb.HasTrigger("CreateLog"));
            // User configuration


            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.User_id);
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Password).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Account_status).HasMaxLength(20).IsRequired();

                entity.HasOne<Subscriptions>()
                    .WithMany()
                    .HasForeignKey(u => u.Subscription_id)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne<Roles>()
                    .WithMany()
                    .HasForeignKey(u => u.Role_id)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne<Users>()
                    .WithMany()
                    .HasForeignKey(u => u.Referred_by)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Content configuration
            modelBuilder.Entity<Contents>(entity =>
            {
                entity.HasKey(e => e.Content_id);
                entity.Property(e => e.Title).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Quality).HasMaxLength(10);
                entity.Property(e => e.Classification).HasMaxLength(50);
            });
            
        
        // Profile configuration
        modelBuilder.Entity<Profiles>(entity =>
            {
                entity.HasKey(e => e.Profile_id);
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Profile_photo).HasMaxLength(255);
                entity.Property(e => e.Language).HasMaxLength(20).HasDefaultValue("English");

                entity.HasOne<Users>()
                    .WithMany()
                    .HasForeignKey(p => p.User_id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Preferences configuration
            modelBuilder.Entity<Preferences>(entity =>
            {
                entity.HasKey(e => e.Preferences_id);
                entity.Property(e => e.Content_restrictions).HasColumnType("text");

                entity.HasOne<Profiles>()
                    .WithMany()
                    .HasForeignKey(p => p.Profile_id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

         

            // Film configuration (inherits from Content)
            modelBuilder.Entity<Films>(entity =>
            {
                entity.HasKey(e => e.Film_id);

                entity.HasOne<Contents>()
                    .WithOne()
                    .HasForeignKey<Films>(f => f.Content_id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Series configuration (inherits from Content)
            modelBuilder.Entity<Series>(entity =>
            {
                entity.HasKey(e => e.Series_id);

                entity.HasOne<Contents>()
                    .WithOne()
                    .HasForeignKey<Series>(s => s.Content_id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Episode configuration
            modelBuilder.Entity<Episodes>(entity =>
            {
                entity.HasKey(e => e.Episode_id);
                entity.Property(e => e.Title).HasMaxLength(100).IsRequired();

                entity.HasOne<Series>()
                    .WithMany()
                    .HasForeignKey(e => e.Series_id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Genre configuration
            modelBuilder.Entity<Genres>(entity =>
            {
                entity.HasKey(e => e.genre_id);
                entity.Property(e => e.genre_name).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Content_Genre>(entity =>
            {
                // Composite primary key
                entity.HasKey(cg => new { cg.content_id, cg.genre_id });

                // Relationships
                entity.HasOne(cg => cg.Content)
                    .WithMany(c => c.ContentGenres)
                    .HasForeignKey(cg => cg.content_id)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(cg => cg.Genre)
                    .WithMany(g => g.ContentGenres)
                    .HasForeignKey(cg => cg.genre_id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Profile_Genre>(entity =>
            {
                // Composite primary key
                entity.HasKey(cg => new { cg.Profile_id, cg.Genre_id });

                // Relationships
                entity.HasOne(cg => cg.Profile)
                    .WithMany(c => c.GenrePreferences)
                    .HasForeignKey(cg => cg.Profile_id)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(cg => cg.Genre)
                    .WithMany(g => g.GenrePreferences)
                    .HasForeignKey(cg => cg.Genre_id)
                    .OnDelete(DeleteBehavior.Cascade);
            });



            // WatchList configuration
            modelBuilder.Entity<WatchLists>(entity =>
            {
                entity.HasKey(e => e.Watchlist_id);

                entity.HasOne<Profiles>()
                    .WithMany()
                    .HasForeignKey(w => w.Profile_id)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<Contents>()
                    .WithMany()
                    .HasForeignKey(w => w.Content_id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // WatchHistory configuration
            modelBuilder.Entity<WatchHistories>(entity =>
            {
                entity.HasKey(e => e.History_id);

                entity.HasOne<Profiles>()
                    .WithMany()
                    .HasForeignKey(w => w.Profile_id)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<Contents>()
                    .WithMany()
                    .HasForeignKey(w => w.Content_id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Subtitle configuration
            modelBuilder.Entity<Subtitles>(entity =>
            {
                entity.HasKey(e => e.Subtitle_id);
                entity.Property(e => e.Language).HasMaxLength(20).IsRequired();

                entity.HasOne<Contents>()
                    .WithMany()
                    .HasForeignKey(s => s.Content_id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Subscription configuration
            modelBuilder.Entity<Subscriptions>(entity =>
            {
                entity.HasKey(e => e.Subscription_id);
                entity.Property(e => e.Plan_type).HasMaxLength(10).IsRequired();
                entity.Property(e => e.Validity_period).HasMaxLength(20).IsRequired();
            });

            // Role configuration
            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.Role_id);
                entity.Property(e => e.Role_name).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Permissions).HasColumnType("text").IsRequired();
            });

            // Referral configuration
            modelBuilder.Entity<Referrals>(entity =>
            {
                entity.HasKey(e => e.Referral_id);
                entity.Property(e => e.Discount_applied).HasDefaultValue(false);

                entity.HasOne<Users>()
                    .WithMany()
                    .HasForeignKey(r => r.Referrer_user_id)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne<Users>()
                    .WithMany()
                    .HasForeignKey(r => r.Referred_user_id)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("trg_DefaultUserRole"));
                entity.ToTable(tb => tb.HasTrigger("trg_UserRoleChange"));
                entity.ToTable(tb => tb.HasTrigger("trg_PreventDuplicateEmails"));
            });

        }
    }
}
