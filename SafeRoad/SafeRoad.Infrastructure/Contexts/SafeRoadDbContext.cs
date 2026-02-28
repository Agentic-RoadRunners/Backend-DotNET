using Microsoft.EntityFrameworkCore;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Enums;
using SafeRoad.Infrastructure.Seeds;

public class SafeRoadDbContext : DbContext
{
      public SafeRoadDbContext(DbContextOptions<SafeRoadDbContext> options)
          : base(options) { }

      // ---- DbSet'ler = Tablolar ----
      public DbSet<User> Users { get; set; }
      public DbSet<Role> Roles { get; set; }
      public DbSet<UserRole> UserRoles { get; set; }
      public DbSet<Incident> Incidents { get; set; }
      public DbSet<IncidentCategory> IncidentCategories { get; set; }
      public DbSet<IncidentPhoto> IncidentPhotos { get; set; }
      public DbSet<Municipality> Municipalities { get; set; }
      public DbSet<Comment> Comments { get; set; }
      public DbSet<Verification> Verifications { get; set; }
      public DbSet<DeviceToken> DeviceTokens { get; set; }
      public DbSet<UserJourney> UserJourneys { get; set; }
      public DbSet<JourneyIncident> JourneyIncidents { get; set; }
      public DbSet<WatchedArea> WatchedAreas { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            base.OnModelCreating(modelBuilder);

            // PostGIS extension for spatial types
            modelBuilder.HasPostgresExtension("postgis");

            // ========== USER ==========
            modelBuilder.Entity<User>(entity =>
            {
                  entity.HasKey(u => u.Id);
                  entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
                  entity.HasIndex(u => u.Email).IsUnique();
                  entity.Property(u => u.PasswordHash).IsRequired();
                  entity.Property(u => u.FullName).HasMaxLength(100);
                  entity.Property(u => u.AvatarUrl).HasMaxLength(500);
                  entity.Property(u => u.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);
            });

            // ========== ROLE ==========
            modelBuilder.Entity<Role>(entity =>
            {
                  entity.HasKey(r => r.Id);
                  entity.Property(r => r.Name).IsRequired().HasMaxLength(50);
            });

            // ========== USER ROLE (Many-to-Many) ==========
            modelBuilder.Entity<UserRole>(entity =>
            {
                  entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                  entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId);

                  entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);
            });

            // ========== INCIDENT ==========
            modelBuilder.Entity<Incident>(entity =>
            {
                  entity.HasKey(i => i.Id);
                  entity.Property(i => i.Title).HasMaxLength(200);
                  entity.Property(i => i.Location).IsRequired();
                  entity.Property(i => i.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                  entity.HasOne(i => i.Reporter)
                    .WithMany(u => u.ReportedIncidents)
                    .HasForeignKey(i => i.ReporterUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                  entity.HasOne(i => i.Category)
                    .WithMany(c => c.Incidents)
                    .HasForeignKey(i => i.CategoryId);

                  entity.HasOne(i => i.Municipality)
                    .WithMany(m => m.Incidents)
                    .HasForeignKey(i => i.MunicipalityId);
            });

            // ========== INCIDENT CATEGORY ==========
            modelBuilder.Entity<IncidentCategory>(entity =>
            {
                  entity.HasKey(c => c.Id);
                  entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });

            // ========== INCIDENT PHOTO ==========
            modelBuilder.Entity<IncidentPhoto>(entity =>
            {
                  entity.HasKey(p => p.Id);
                  entity.Property(p => p.BlobUrl).IsRequired().HasMaxLength(500);

                  entity.HasOne(p => p.Incident)
                    .WithMany(i => i.Photos)
                    .HasForeignKey(p => p.IncidentId);
            });

            // ========== MUNICIPALITY ==========
            modelBuilder.Entity<Municipality>(entity =>
            {
                  entity.HasKey(m => m.Id);
                  entity.Property(m => m.Name).IsRequired().HasMaxLength(100);
                  entity.Property(m => m.Boundary);
            });

            // ========== COMMENT ==========
            modelBuilder.Entity<Comment>(entity =>
            {
                  entity.HasKey(c => c.Id);
                  entity.Property(c => c.Content).IsRequired();

                  entity.HasOne(c => c.Incident)
                    .WithMany(i => i.Comments)
                    .HasForeignKey(c => c.IncidentId);

                  entity.HasOne(c => c.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c => c.UserId);
            });

            // ========== VERIFICATION ==========
            modelBuilder.Entity<Verification>(entity =>
            {
                  entity.HasKey(v => v.Id);
                  entity.Property(v => v.IsPositive).IsRequired();

                  entity.HasOne(v => v.Incident)
                    .WithMany(i => i.Verifications)
                    .HasForeignKey(v => v.IncidentId);

                  entity.HasOne(v => v.User)
                    .WithMany(u => u.Verifications)
                    .HasForeignKey(v => v.UserId);
            });

            // ========== DEVICE TOKEN ==========
            modelBuilder.Entity<DeviceToken>(entity =>
            {
                  entity.HasKey(d => d.Id);
                  entity.Property(d => d.TokenString).IsRequired();
                  entity.Property(d => d.DeviceType).HasMaxLength(20);

                  entity.HasOne(d => d.User)
                    .WithMany(u => u.DeviceTokens)
                    .HasForeignKey(d => d.UserId);
            });

            // ========== USER JOURNEY ==========
            modelBuilder.Entity<UserJourney>(entity =>
            {
                  entity.HasKey(j => j.Id);
                  entity.Property(j => j.RoutePath).IsRequired();
                  entity.Property(j => j.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                  entity.HasOne(j => j.User)
                    .WithMany(u => u.UserJourneys)
                    .HasForeignKey(j => j.UserId);
            });

            // ========== JOURNEY INCIDENT ==========
            modelBuilder.Entity<JourneyIncident>(entity =>
            {
                  entity.HasKey(ji => ji.Id);

                  entity.HasOne(ji => ji.Journey)
                    .WithMany(j => j.JourneyIncidents)
                    .HasForeignKey(ji => ji.JourneyId);

                  entity.HasOne(ji => ji.Incident)
                    .WithMany(i => i.JourneyIncidents)
                    .HasForeignKey(ji => ji.IncidentId);
            });

            // ========== WATCHED AREA ==========
            modelBuilder.Entity<WatchedArea>(entity =>
            {
                  entity.HasKey(w => w.Id);
                  entity.Property(w => w.Label).HasMaxLength(50);
                  entity.Property(w => w.Area).IsRequired();

                  entity.HasOne(w => w.User)
                    .WithMany(u => u.WatchedAreas)
                    .HasForeignKey(w => w.UserId);
            });

            // ========== SEED DATA ==========
            RoleSeedData.Seed(modelBuilder);
            MunicipalitySeedData.Seed(modelBuilder);
            IncidentCategorySeedData.Seed(modelBuilder);
            UserSeedData.Seed(modelBuilder);
            UserRoleSeedData.Seed(modelBuilder);
            IncidentSeedData.Seed(modelBuilder);
            CommentSeedData.Seed(modelBuilder);
            VerificationSeedData.Seed(modelBuilder);
      }
}