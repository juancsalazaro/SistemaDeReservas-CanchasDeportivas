using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SistemaReservasApi.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required]
        public int CanchaId { get; set; }
        public Cancha Cancha { get; set; } = null!;

        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public DateTime FechaReserva { get; set; }

        [Required]
        public DateTime HoraInicio { get; set; }

        [Required]
        public DateTime HoraFin { get; set; }

        [Required]
        public decimal PrecioTotal { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; } = "Pendiente";

        [Required]
        [StringLength(100)]
        public string NombreCliente { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string EmailCliente { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string TelefonoCliente { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string MetodoPago { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Observaciones { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}