using Microsoft.EntityFrameworkCore;
using SistemaReservasApi.Models;

namespace SistemaReservasApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Cancha> Canchas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para la tabla users
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.Property(e => e.Id).UseIdentityByDefaultColumn();
                entity.Property(e => e.Username).HasMaxLength(100).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.IsActive).HasColumnType("boolean").HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasColumnType("timestamp with time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");
                entity.HasIndex(e => e.Username).IsUnique().HasDatabaseName("IX_users_username");
            });

            // Configuración para la tabla canchas
            modelBuilder.Entity<Cancha>(entity =>
            {
                entity.ToTable("canchas");
                entity.Property(e => e.Id).UseIdentityByDefaultColumn();

                entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Descripcion).HasMaxLength(500).IsRequired();
                entity.Property(e => e.TipoDeporte).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Ubicacion).HasMaxLength(200).IsRequired();

                entity.Property(e => e.PrecioPorHora).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.Calificacion).HasColumnType("decimal(3,2)").HasDefaultValue(0);
                entity.Property(e => e.NumeroCalificaciones).HasDefaultValue(0);

                entity.Property(e => e.ImagenPrincipal).HasColumnType("text");
                entity.Property(e => e.ImagenesAdicionales).HasColumnType("text");
                entity.Property(e => e.Amenidades).HasColumnType("text");

                entity.Property(e => e.Disponible).HasColumnType("boolean").HasDefaultValue(true);

                entity.Property(e => e.CreatedAt).HasColumnType("timestamp with time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");

                // Índices
                entity.HasIndex(e => e.Nombre).IsUnique().HasDatabaseName("IX_canchas_nombre");
                entity.HasIndex(e => e.TipoDeporte).HasDatabaseName("IX_canchas_tipo_deporte");
                entity.HasIndex(e => e.Disponible).HasDatabaseName("IX_canchas_disponible");

                // Relación con User
                entity.HasOne(c => c.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(c => c.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}