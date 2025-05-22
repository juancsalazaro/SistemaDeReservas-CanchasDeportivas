using Microsoft.EntityFrameworkCore;
using SistemaReservasApi.Models;

namespace SistemaReservasApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id)
                    .UseIdentityByDefaultColumn();

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.PasswordHash)
                    .IsRequired();

                entity.Property(e => e.IsActive)
                    .HasColumnType("boolean")
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp with time zone");

                entity.HasIndex(e => e.Username)
                    .IsUnique()
                    .HasDatabaseName("IX_users_username");
            });
        }
    }
}