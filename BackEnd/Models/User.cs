using Microsoft.EntityFrameworkCore;
using SistemaReservasApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SistemaReservasApi.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Rol { get; set; } = UserRole.Cliente;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;
    }
}