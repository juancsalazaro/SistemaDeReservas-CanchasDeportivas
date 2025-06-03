namespace SistemaReservasApi.Dtos
{
    public class CanchaDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string TipoDeporte { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public decimal PrecioPorHora { get; set; }
        public string? ImagenPrincipal { get; set; }
        public string? ImagenesAdicionales { get; set; }
        public string? Amenidades { get; set; }
        public bool Disponible { get; set; } = true;
    }

    public class CanchaResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string TipoDeporte { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public decimal PrecioPorHora { get; set; }
        public string? ImagenPrincipal { get; set; }
        public string? ImagenesAdicionales { get; set; }
        public decimal Calificacion { get; set; }
        public int NumeroCalificaciones { get; set; }
        public bool Disponible { get; set; }
        public string? Amenidades { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUsername { get; set; } = string.Empty;
    }
}