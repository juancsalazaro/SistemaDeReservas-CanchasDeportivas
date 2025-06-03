using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SistemaReservasApi.Models
{
    [Index(nameof(Nombre), IsUnique = true)]
    public class Cancha
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string TipoDeporte { get; set; } = string.Empty; // Futbol, Tenis, Basquet, etc.

        [Required, MaxLength(200)]
        public string Ubicacion { get; set; } = string.Empty;

        [Required]
        public decimal PrecioPorHora { get; set; }

        public string? ImagenPrincipal { get; set; }

        public string? ImagenesAdicionales { get; set; } // JSON array de URLs

        public decimal Calificacion { get; set; } = 0;

        public int NumeroCalificaciones { get; set; } = 0;

        public bool Disponible { get; set; } = true;

        public string? Amenidades { get; set; } // JSON array: ["Estacionamiento", "Vestuarios", etc.]

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Relación con el usuario que creó la cancha (para futuros admin)
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;
    }
}