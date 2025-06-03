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
    public class CanchasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CanchasController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las canchas disponibles
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Obtener todas las canchas", Description = "Devuelve una lista de todas las canchas disponibles")]
        [ProducesResponseType(typeof(IEnumerable<CanchaResponseDto>), 200)]
        public async Task<ActionResult<IEnumerable<CanchaResponseDto>>> GetCanchas(
            [FromQuery] string? tipoDeporte = null,
            [FromQuery] bool? disponible = null,
            [FromQuery] decimal? precioMaximo = null)
        {
            var query = _context.Canchas
                .Include(c => c.CreatedByUser)
                .AsQueryable();

            // Filtros opcionales
            if (!string.IsNullOrEmpty(tipoDeporte))
            {
                query = query.Where(c => c.TipoDeporte.ToLower().Contains(tipoDeporte.ToLower()));
            }

            if (disponible.HasValue)
            {
                query = query.Where(c => c.Disponible == disponible.Value);
            }

            if (precioMaximo.HasValue)
            {
                query = query.Where(c => c.PrecioPorHora <= precioMaximo.Value);
            }

            var canchas = await query
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new CanchaResponseDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    TipoDeporte = c.TipoDeporte,
                    Ubicacion = c.Ubicacion,
                    PrecioPorHora = c.PrecioPorHora,
                    ImagenPrincipal = c.ImagenPrincipal,
                    ImagenesAdicionales = c.ImagenesAdicionales,
                    Calificacion = c.Calificacion,
                    NumeroCalificaciones = c.NumeroCalificaciones,
                    Disponible = c.Disponible,
                    Amenidades = c.Amenidades,
                    CreatedAt = c.CreatedAt,
                    CreatedByUsername = c.CreatedByUser.Username
                })
                .ToListAsync();

            return Ok(canchas);
        }

        /// <summary>
        /// Obtiene una cancha por ID
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtener cancha por ID")]
        [ProducesResponseType(typeof(CanchaResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CanchaResponseDto>> GetCancha(int id)
        {
            var cancha = await _context.Canchas
                .Include(c => c.CreatedByUser)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cancha == null)
            {
                return NotFound(new { message = "Cancha no encontrada" });
            }

            var canchaDto = new CanchaResponseDto
            {
                Id = cancha.Id,
                Nombre = cancha.Nombre,
                Descripcion = cancha.Descripcion,
                TipoDeporte = cancha.TipoDeporte,
                Ubicacion = cancha.Ubicacion,
                PrecioPorHora = cancha.PrecioPorHora,
                ImagenPrincipal = cancha.ImagenPrincipal,
                ImagenesAdicionales = cancha.ImagenesAdicionales,
                Calificacion = cancha.Calificacion,
                NumeroCalificaciones = cancha.NumeroCalificaciones,
                Disponible = cancha.Disponible,
                Amenidades = cancha.Amenidades,
                CreatedAt = cancha.CreatedAt,
                CreatedByUsername = cancha.CreatedByUser.Username
            };

            return Ok(canchaDto);
        }

        /// <summary>
        /// Crear una nueva cancha (requiere autenticación)
        /// </summary>
        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Crear nueva cancha")]
        [ProducesResponseType(typeof(CanchaResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<CanchaResponseDto>> CreateCancha([FromBody] CanchaDto canchaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Obtener el ID del usuario autenticado
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            // Verificar que el nombre de la cancha no exista
            var existeCancha = await _context.Canchas
                .AnyAsync(c => c.Nombre.ToLower() == canchaDto.Nombre.ToLower());

            if (existeCancha)
            {
                return BadRequest(new { message = "Ya existe una cancha con ese nombre" });
            }

            var cancha = new Cancha
            {
                Nombre = canchaDto.Nombre,
                Descripcion = canchaDto.Descripcion,
                TipoDeporte = canchaDto.TipoDeporte,
                Ubicacion = canchaDto.Ubicacion,
                PrecioPorHora = canchaDto.PrecioPorHora,
                ImagenPrincipal = canchaDto.ImagenPrincipal,
                ImagenesAdicionales = canchaDto.ImagenesAdicionales,
                Amenidades = canchaDto.Amenidades,
                Disponible = canchaDto.Disponible,
                CreatedByUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Canchas.Add(cancha);
            await _context.SaveChangesAsync();

            // Cargar el usuario para la respuesta
            await _context.Entry(cancha)
                .Reference(c => c.CreatedByUser)
                .LoadAsync();

            var responseDto = new CanchaResponseDto
            {
                Id = cancha.Id,
                Nombre = cancha.Nombre,
                Descripcion = cancha.Descripcion,
                TipoDeporte = cancha.TipoDeporte,
                Ubicacion = cancha.Ubicacion,
                PrecioPorHora = cancha.PrecioPorHora,
                ImagenPrincipal = cancha.ImagenPrincipal,
                ImagenesAdicionales = cancha.ImagenesAdicionales,
                Calificacion = cancha.Calificacion,
                NumeroCalificaciones = cancha.NumeroCalificaciones,
                Disponible = cancha.Disponible,
                Amenidades = cancha.Amenidades,
                CreatedAt = cancha.CreatedAt,
                CreatedByUsername = cancha.CreatedByUser.Username
            };

            return CreatedAtAction(nameof(GetCancha), new { id = cancha.Id }, responseDto);
        }

        /// <summary>
        /// Obtener tipos de deportes disponibles
        /// </summary>
        [HttpGet("tipos-deporte")]
        [SwaggerOperation(Summary = "Obtener tipos de deportes")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public async Task<ActionResult<IEnumerable<string>>> GetTiposDeporte()
        {
            var tipos = await _context.Canchas
                .Where(c => c.Disponible)
                .Select(c => c.TipoDeporte)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            return Ok(tipos);
        }
    }
}