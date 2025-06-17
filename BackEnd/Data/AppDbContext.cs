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
        public DbSet<Reserva> Reservas { get; set; } // NUEVO

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

            // Configuración para la tabla reservas (NUEVO)
            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.ToTable("reservas");
                entity.Property(e => e.Id).UseIdentityByDefaultColumn();

                // Campos básicos de reserva
                entity.Property(e => e.FechaReserva).HasColumnType("date").IsRequired();
                entity.Property(e => e.HoraInicio).HasColumnType("timestamp with time zone").IsRequired();
                entity.Property(e => e.HoraFin).HasColumnType("timestamp with time zone").IsRequired();
                entity.Property(e => e.PrecioTotal).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.Estado).HasMaxLength(50).IsRequired().HasDefaultValue("Pendiente");

                // Datos del cliente
                entity.Property(e => e.NombreCliente).HasMaxLength(100).IsRequired();
                entity.Property(e => e.EmailCliente).HasMaxLength(150).IsRequired();
                entity.Property(e => e.TelefonoCliente).HasMaxLength(20).IsRequired();

                // Método de pago
                entity.Property(e => e.MetodoPago).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Observaciones).HasMaxLength(500);

                // Timestamps
                entity.Property(e => e.CreatedAt).HasColumnType("timestamp with time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");

                // Índices
                entity.HasIndex(e => e.FechaReserva).HasDatabaseName("IX_reservas_fecha");
                entity.HasIndex(e => e.Estado).HasDatabaseName("IX_reservas_estado");
                entity.HasIndex(e => new { e.CanchaId, e.FechaReserva }).HasDatabaseName("IX_reservas_cancha_fecha");

                // Relaciones
                entity.HasOne(r => r.Cancha)
                    .WithMany()
                    .HasForeignKey(r => r.CanchaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}