﻿using SistemaReservasApi.Enums;

namespace SistemaReservasApi.Dtos
{
    public class UserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole? Rol { get; set; }
    }
}