using System.ComponentModel.DataAnnotations;

namespace SistemaReservasApi.Dtos
{
    public class ReservaDto
    {
        [Required]
        public int CanchaId { get; set; }

        [Required]
        public DateTime FechaReserva { get; set; }

        [Required]
        public DateTime HoraInicio { get; set; }

        [Required]
        public DateTime HoraFin { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreCliente { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string EmailCliente { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string TelefonoCliente { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Observaciones { get; set; }

        // Datos del pago simulado
        [Required]
        public PagoSimuladoDto DatosPago { get; set; } = new();
    }

    public class PagoSimuladoDto
    {
        [Required]
        [StringLength(50)]
        public string TipoTarjeta { get; set; } = string.Empty;

        [Required]
        [StringLength(19)]
        public string NumeroTarjeta { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string NombreTarjeta { get; set; } = string.Empty;

        [Required]
        [StringLength(5)]
        public string FechaVencimiento { get; set; } = string.Empty;

        [Required]
        [StringLength(4)]
        public string CVV { get; set; } = string.Empty;
    }

    public class ReservaResponseDto
    {
        public int Id { get; set; }
        public int CanchaId { get; set; }
        public string NombreCancha { get; set; } = string.Empty;
        public DateTime FechaReserva { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
        public decimal PrecioTotal { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public string EmailCliente { get; set; } = string.Empty;
        public string TelefonoCliente { get; set; } = string.Empty;
        public string MetodoPago { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Username { get; set; } = string.Empty;
    }

    public class DisponibilidadDto
    {
        [Required]
        public int CanchaId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }
    }

    public class DisponibilidadResponseDto
    {
        public DateTime Fecha { get; set; }
        public List<HorarioDisponibleDto> HorariosDisponibles { get; set; } = new();
        public List<ReservaActivaDto> ReservasActivas { get; set; } = new();
    }

    public class HorarioDisponibleDto
    {
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public bool Disponible { get; set; }
    }

    public class ReservaActivaDto
    {
        public int Id { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
    }
}