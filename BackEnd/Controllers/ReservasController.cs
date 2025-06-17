using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using SistemaReservasApi.Data;
using SistemaReservasApi.Models;
using SistemaReservasApi.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace SistemaReservasApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ReservasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReservasController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Crear una nueva reserva
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Crear nueva reserva", Description = "Crea una reserva y procesa el pago simulado")]
        [ProducesResponseType(typeof(ReservaResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ReservaResponseDto>> CrearReserva([FromBody] ReservaDto reservaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Obtener el ID del usuario autenticado
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            // Verificar que la cancha existe y está disponible
            var cancha = await _context.Canchas
                .FirstOrDefaultAsync(c => c.Id == reservaDto.CanchaId && c.Disponible);

            if (cancha == null)
            {
                return BadRequest(new { message = "La cancha no existe o no está disponible" });
            }

            // Usar directamente los DateTime del DTO
            var fechaReserva = reservaDto.FechaReserva.Date; // Solo la fecha
            var horaInicio = reservaDto.HoraInicio;
            var horaFin = reservaDto.HoraFin;

            // Validar que la hora de fin sea posterior a la de inicio
            if (horaFin <= horaInicio)
            {
                return BadRequest(new { message = "La hora de fin debe ser posterior a la hora de inicio" });
            }

            // Verificar disponibilidad del horario
            var conflicto = await _context.Reservas
                .AnyAsync(r => r.CanchaId == reservaDto.CanchaId
                             && r.FechaReserva.Date == fechaReserva
                             && r.Estado != "Cancelada"
                             && ((horaInicio >= r.HoraInicio && horaInicio < r.HoraFin) ||
                                 (horaFin > r.HoraInicio && horaFin <= r.HoraFin) ||
                                 (horaInicio <= r.HoraInicio && horaFin >= r.HoraFin)));

            if (conflicto)
            {
                return BadRequest(new { message = "El horario seleccionado ya está reservado" });
            }

            // Calcular precio total
            var horas = (horaFin - horaInicio).TotalHours;
            var precioTotal = (decimal)horas * cancha.PrecioPorHora;

            // Simular procesamiento de pago (siempre exitoso para demo)
            var numeroTarjetaLimpio = reservaDto.DatosPago.NumeroTarjeta.Replace(" ", "");
            var metodoPago = $"{reservaDto.DatosPago.TipoTarjeta} ****{numeroTarjetaLimpio.Substring(Math.Max(0, numeroTarjetaLimpio.Length - 4))}";

            // Crear la reserva
            var reserva = new Reserva
            {
                CanchaId = reservaDto.CanchaId,
                UserId = userId,
                FechaReserva = fechaReserva,
                HoraInicio = horaInicio,
                HoraFin = horaFin,
                PrecioTotal = precioTotal,
                Estado = "Confirmada", // Pago simulado siempre exitoso
                NombreCliente = reservaDto.NombreCliente,
                EmailCliente = reservaDto.EmailCliente,
                TelefonoCliente = reservaDto.TelefonoCliente,
                MetodoPago = metodoPago,
                Observaciones = reservaDto.Observaciones,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            // Cargar datos relacionados para la respuesta
            await _context.Entry(reserva)
                .Reference(r => r.Cancha)
                .LoadAsync();
            await _context.Entry(reserva)
                .Reference(r => r.User)
                .LoadAsync();

            var responseDto = new ReservaResponseDto
            {
                Id = reserva.Id,
                CanchaId = reserva.CanchaId,
                NombreCancha = reserva.Cancha.Nombre,
                FechaReserva = reserva.FechaReserva,
                HoraInicio = reserva.HoraInicio,
                HoraFin = reserva.HoraFin,
                PrecioTotal = reserva.PrecioTotal,
                Estado = reserva.Estado,
                NombreCliente = reserva.NombreCliente,
                EmailCliente = reserva.EmailCliente,
                TelefonoCliente = reserva.TelefonoCliente,
                MetodoPago = reserva.MetodoPago,
                Observaciones = reserva.Observaciones,
                CreatedAt = reserva.CreatedAt,
                Username = reserva.User.Username
            };

            return CreatedAtAction(nameof(GetReserva), new { id = reserva.Id }, responseDto);
        }

        /// <summary>
        /// Obtener reservas del usuario autenticado
        /// </summary>
        [HttpGet("mis-reservas")]
        [SwaggerOperation(Summary = "Obtener mis reservas")]
        [ProducesResponseType(typeof(IEnumerable<ReservaResponseDto>), 200)]
        public async Task<ActionResult<IEnumerable<ReservaResponseDto>>> GetMisReservas()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            var reservas = await _context.Reservas
                .Include(r => r.Cancha)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReservaResponseDto
                {
                    Id = r.Id,
                    CanchaId = r.CanchaId,
                    NombreCancha = r.Cancha.Nombre,
                    FechaReserva = r.FechaReserva,
                    HoraInicio = r.HoraInicio,
                    HoraFin = r.HoraFin,
                    PrecioTotal = r.PrecioTotal,
                    Estado = r.Estado,
                    NombreCliente = r.NombreCliente,
                    EmailCliente = r.EmailCliente,
                    TelefonoCliente = r.TelefonoCliente,
                    MetodoPago = r.MetodoPago,
                    Observaciones = r.Observaciones,
                    CreatedAt = r.CreatedAt,
                    Username = r.User.Username
                })
                .ToListAsync();

            return Ok(reservas);
        }

        /// <summary>
        /// Obtener una reserva específica
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtener reserva por ID")]
        [ProducesResponseType(typeof(ReservaResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ReservaResponseDto>> GetReserva(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            var reserva = await _context.Reservas
                .Include(r => r.Cancha)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (reserva == null)
            {
                return NotFound(new { message = "Reserva no encontrada" });
            }

            var responseDto = new ReservaResponseDto
            {
                Id = reserva.Id,
                CanchaId = reserva.CanchaId,
                NombreCancha = reserva.Cancha.Nombre,
                FechaReserva = reserva.FechaReserva,
                HoraInicio = reserva.HoraInicio,
                HoraFin = reserva.HoraFin,
                PrecioTotal = reserva.PrecioTotal,
                Estado = reserva.Estado,
                NombreCliente = reserva.NombreCliente,
                EmailCliente = reserva.EmailCliente,
                TelefonoCliente = reserva.TelefonoCliente,
                MetodoPago = reserva.MetodoPago,
                Observaciones = reserva.Observaciones,
                CreatedAt = reserva.CreatedAt,
                Username = reserva.User.Username
            };

            return Ok(responseDto);
        }

        /// <summary>
        /// Obtener disponibilidad de una cancha en una fecha
        /// </summary>
        [HttpGet("disponibilidad")]
        [SwaggerOperation(Summary = "Consultar disponibilidad")]
        [ProducesResponseType(typeof(DisponibilidadResponseDto), 200)]
        public async Task<ActionResult<DisponibilidadResponseDto>> GetDisponibilidad(
            [FromQuery] int canchaId,
            [FromQuery] DateTime fecha)
        {
            // Verificar que la cancha existe
            var cancha = await _context.Canchas.FindAsync(canchaId);
            if (cancha == null)
            {
                return NotFound(new { message = "Cancha no encontrada" });
            }

            // Obtener reservas activas para esa fecha
            var reservasActivas = await _context.Reservas
                .Include(r => r.User)
                .Where(r => r.CanchaId == canchaId
                           && r.FechaReserva.Date == fecha.Date
                           && r.Estado != "Cancelada")
                .OrderBy(r => r.HoraInicio)
                .Select(r => new ReservaActivaDto
                {
                    Id = r.Id,
                    HoraInicio = r.HoraInicio,
                    HoraFin = r.HoraFin,
                    Estado = r.Estado,
                    NombreCliente = r.NombreCliente
                })
                .ToListAsync();

            // Generar horarios disponibles (6 AM a 10 PM)
            var horariosDisponibles = new List<HorarioDisponibleDto>();
            for (int hora = 6; hora <= 22; hora++)
            {
                var horaInicio = new TimeSpan(hora, 0, 0);
                var horaFin = new TimeSpan(hora + 1, 0, 0);

                // Verificar si está ocupado
                var ocupado = reservasActivas.Any(r =>
                {
                    var reservaInicio = r.HoraInicio.TimeOfDay;
                    var reservaFin = r.HoraFin.TimeOfDay;
                    return (horaInicio >= reservaInicio && horaInicio < reservaFin) ||
                           (horaFin > reservaInicio && horaFin <= reservaFin) ||
                           (horaInicio <= reservaInicio && horaFin >= reservaFin);
                });

                horariosDisponibles.Add(new HorarioDisponibleDto
                {
                    HoraInicio = horaInicio,
                    HoraFin = horaFin,
                    Disponible = !ocupado
                });
            }

            var responseDto = new DisponibilidadResponseDto
            {
                Fecha = fecha.Date,
                HorariosDisponibles = horariosDisponibles,
                ReservasActivas = reservasActivas
            };

            return Ok(responseDto);
        }

        /// <summary>
        /// Cancelar una reserva
        /// </summary>
        [HttpPatch("{id}/cancelar")]
        [SwaggerOperation(Summary = "Cancelar reserva")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (reserva == null)
            {
                return NotFound(new { message = "Reserva no encontrada" });
            }

            if (reserva.Estado == "Cancelada")
            {
                return BadRequest(new { message = "La reserva ya está cancelada" });
            }

            if (reserva.Estado == "Completada")
            {
                return BadRequest(new { message = "No se puede cancelar una reserva completada" });
            }

            // Verificar que se puede cancelar (al menos 2 horas antes)
            var horasAntes = (reserva.HoraInicio - DateTime.UtcNow).TotalHours;
            if (horasAntes < 2)
            {
                return BadRequest(new { message = "No se puede cancelar con menos de 2 horas de anticipación" });
            }

            reserva.Estado = "Cancelada";
            reserva.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Reserva cancelada exitosamente" });
        }
    }
}