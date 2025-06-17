## 📋 Descripción

Este proyecto permite a los usuarios registrarse, iniciar sesión y gestionar reservas de canchas deportivas de manera intuitiva. El sistema cuenta con una arquitectura robusta dividida en backend y frontend, ofreciendo una experiencia de usuario fluida y moderna con **sistema de reservas completo**.

## 🚀 Tecnologías Utilizadas

### Backend
- **.NET 8** - Framework principal
- **PostgreSQL** - Base de datos principal
- **Entity Framework Core** - ORM
- **JWT** - Autenticación y autorización
- **BCrypt** - Encriptación de contraseñas
- **Swagger** - Documentación de API

### Frontend
- **Angular 18** - Framework de frontend
- **TypeScript** - Lenguaje de programación
- **Bootstrap** - Framework CSS
- **Font Awesome** - Iconografía
- **Reactive Forms** - Manejo de formularios

## 📁 Estructura del Proyecto

```
SistemaDeReservas-CanchasDeportivas/
├── BackEnd/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── CanchasController.cs
│   │   └── ReservasController.cs          # ✅ NUEVO
│   ├── Data/
│   │   └── AppDbContext.cs
│   ├── Models/
│   │   ├── User.cs
│   │   ├── Cancha.cs
│   │   └── Reserva.cs                     # ✅ NUEVO
│   ├── Dtos/
│   │   ├── UserDto.cs
│   │   ├── CanchaDto.cs
│   │   ├── CanchaResponseDto.cs
│   │   ├── ReservaDto.cs                  # ✅ NUEVO
│   │   ├── ReservaResponseDto.cs          # ✅ NUEVO
│   │   ├── DisponibilidadResponseDto.cs   # ✅ NUEVO
│   │   └── PagoSimuladoDto.cs            # ✅ NUEVO
│   ├── Migrations/
│   └── Program.cs
├── FrontEnd/
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/
│   │   │   │   ├── auth.guard.ts
│   │   │   │   ├── auth.interceptor.ts
│   │   │   │   └── auth.service.ts
│   │   │   ├── features/
│   │   │   │   ├── login/
│   │   │   │   ├── register/
│   │   │   │   ├── dashboard/
│   │   │   │   └── reservas/             # ✅ NUEVO
│   │   │   ├── models/
│   │   │   │   ├── user.dto.ts
│   │   │   │   ├── cancha.dto.ts
│   │   │   │   └── reserva.dto.ts        # ✅ NUEVO
│   │   │   └── services/
│   │   │       ├── canchas.service.ts
│   │   │       └── reservas.service.ts   # ✅ NUEVO
│   │   └── assets/
└── README.md
```

## ⚙️ Instalación y Configuración

### Prerrequisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [PostgreSQL 14+](https://www.postgresql.org/)
- [Angular CLI](https://angular.io/cli)

### 🗄️ Configuración del Backend

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/juancsalazaro/SistemaDeReservas-CanchasDeportivas-.git
   cd SistemaDeReservas-CanchasDeportivas-
   ```

2. **Configurar PostgreSQL**
   ```sql
   -- Crear base de datos
   CREATE DATABASE ReservasCanchas;
   
   -- Crear usuario (opcional)
   CREATE USER reservas_user WITH PASSWORD 'tu_password';
   GRANT ALL PRIVILEGES ON DATABASE ReservasCanchas TO reservas_user;
   ```

3. **Configurar cadena de conexión**
   
   Actualizar `BackEnd/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=ReservasCanchas;Username=postgres;Password=tu_password;Port=5432"
     },
     "Jwt": {
       "Key": "TuClaveSecretaMuySeguraAqui123456789"
     }
   }
   ```

4. **Instalar dependencias y ejecutar migraciones**
   ```bash
   cd BackEnd
   dotnet restore
   dotnet ef database update
   ```

5. **Ejecutar el backend**
   ```bash
   dotnet run
   ```
   
   El API estará disponible en: `http://localhost:5286`

### 🎨 Configuración del Frontend

1. **Instalar dependencias**
   ```bash
   cd FrontEnd
   npm install
   ```

2. **Ejecutar el frontend**
   ```bash
   ng serve
   ```
   
   La aplicación estará disponible en: `http://localhost:4200`

## 🔧 Endpoints de la API

### Autenticación

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/api/v1/auth/register` | Registrar nuevo usuario |
| POST | `/api/v1/auth/login` | Iniciar sesión |

### Canchas

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/v1/canchas` | Obtener todas las canchas con filtros |
| GET | `/api/v1/canchas/{id}` | Obtener cancha por ID |
| POST | `/api/v1/canchas` | Crear nueva cancha (requiere autenticación) |
| GET | `/api/v1/canchas/tipos-deporte` | Obtener tipos de deportes disponibles |

### 🆕 Reservas

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/api/v1/reservas` | Crear nueva reserva |
| GET | `/api/v1/reservas/mis-reservas` | Obtener reservas del usuario autenticado |
| GET | `/api/v1/reservas/{id}` | Obtener reserva específica por ID |
| GET | `/api/v1/reservas/disponibilidad` | Consultar disponibilidad de cancha por fecha |
| PATCH | `/api/v1/reservas/{id}/cancelar` | Cancelar una reserva |

### Ejemplo de uso

**Registro:**
```json
POST /api/v1/auth/register
{
  "username": "usuario123",
  "password": "contraseña123"
}
```

**Login:**
```json
POST /api/v1/auth/login
{
  "username": "usuario123",
  "password": "contraseña123"
}
```

**Obtener canchas con filtros:**
```bash
GET /api/v1/canchas?tipoDeporte=Futbol&precioMaximo=50000&disponible=true
```

**Crear cancha:**
```json
POST /api/v1/canchas
Authorization: Bearer {token}
{
  "nombre": "Cancha El Estadio",
  "descripcion": "Cancha de fútbol profesional con césped sintético",
  "tipoDeporte": "Futbol",
  "ubicacion": "Estadio La Nubia, Manizales",
  "precioPorHora": 80000,
  "amenidades": "[\"Vestuarios\", \"Estacionamiento\", \"Iluminación\"]"
}
```

**🆕 Crear reserva:**
```json
POST /api/v1/reservas
Authorization: Bearer {token}
{
  "canchaId": 1,
  "fechaReserva": "2024-07-15T00:00:00",
  "horaInicio": "2024-07-15T14:00:00",
  "horaFin": "2024-07-15T16:00:00",
  "nombreCliente": "Juan Pérez",
  "emailCliente": "juan@email.com",
  "telefonoCliente": "123456789",
  "observaciones": "Reserva para partido amistoso",
  "datosPago": {
    "tipoTarjeta": "Visa",
    "numeroTarjeta": "4111 1111 1111 1111",
    "nombreTarjeta": "Juan Pérez",
    "fechaVencimiento": "12/25",
    "cvv": "123"
  }
}
```

**🆕 Consultar disponibilidad:**
```bash
GET /api/v1/reservas/disponibilidad?canchaId=1&fecha=2024-07-15T00:00:00
```

## 🎯 Funcionalidades Implementadas

### ✅ Autenticación y Seguridad
- **Registro de usuarios** con validaciones robustas
- **Inicio de sesión** con JWT
- **Encriptación de contraseñas** con BCrypt
- **Guards y interceptores** para protección de rutas

### ✅ Gestión de Canchas
- **CRUD completo** de canchas deportivas
- **Filtros avanzados** por deporte, precio y disponibilidad
- **Búsqueda por ubicación** integrada
- **Categorización** por tipos de deporte
- **Sistema de calificaciones** y reseñas

### 🆕 ✅ Sistema de Reservas
- **Creación de reservas** con validación de disponibilidad
- **Gestión de horarios** por bloques de tiempo
- **Verificación de conflictos** de reservas
- **Consulta de disponibilidad** en tiempo real
- **Cancelación de reservas** con restricciones de tiempo
- **Historial de reservas** por usuario
- **Simulación de pagos** con múltiples métodos
- **Cálculo automático** de precios por hora
- **Estados de reserva** (Confirmada, Cancelada, Completada)

### ✅ Dashboard Moderno
- **Interfaz intuitiva** y responsive
- **Buscador avanzado** con múltiples filtros
- **Grid adaptativo** de canchas
- **Cards interactivas** con efectos visuales
- **Categorías deportivas** con iconos
- **Estados de carga** informativos

### ✅ Experiencia de Usuario
- **Diseño responsive** para todos los dispositivos
- **Validación de formularios** en tiempo real
- **Manejo de errores** con feedback visual
- **Animaciones suaves** y transiciones
- **Estados de carga** informativos
- **Navegación intuitiva** entre secciones

### ✅ Tecnología y Arquitectura
- **API REST** bien estructurada
- **Base de datos PostgreSQL** optimizada
- **Componentes standalone** en Angular
- **Servicios modulares** y reutilizables
- **Arquitectura escalable** y mantenible

## 🗄️ Modelo de Base de Datos

### Tabla: Users
```sql
- Id (int, PK)
- Username (varchar, unique)
- PasswordHash (varchar)
- CreatedAt (timestamp)
- UpdatedAt (timestamp)
```

### Tabla: Canchas
```sql
- Id (int, PK)
- Nombre (varchar)
- Descripcion (text)
- TipoDeporte (varchar)
- Ubicacion (varchar)
- PrecioPorHora (decimal)
- ImagenPrincipal (varchar)
- ImagenesAdicionales (json)
- Calificacion (decimal)
- NumeroCalificaciones (int)
- Disponible (boolean)
- Amenidades (json)
- CreatedByUserId (int, FK)
- CreatedAt (timestamp)
```

### 🆕 Tabla: Reservas
```sql
- Id (int, PK)
- CanchaId (int, FK)
- UserId (int, FK)
- FechaReserva (date)
- HoraInicio (timestamp)
- HoraFin (timestamp)
- PrecioTotal (decimal)
- Estado (varchar) -- Confirmada, Cancelada, Completada
- NombreCliente (varchar)
- EmailCliente (varchar)
- TelefonoCliente (varchar)
- MetodoPago (varchar)
- Observaciones (text)
- CreatedAt (timestamp)
- UpdatedAt (timestamp)
```

## 🎨 Capturas de Pantalla

### Dashboard Principal
*Interfaz principal con búsqueda avanzada y grid de canchas deportivas*

### Sistema de Reservas
*Formulario de reserva con selección de fecha/hora y simulación de pago*

### Gestión de Reservas
*Panel para ver, gestionar y cancelar reservas del usuario*

### Login/Register
*Formularios modernos con validaciones en tiempo real y diseño responsive*

### Gestión de Canchas
*CRUD completo con filtros y categorización por deportes*

## 🔍 Características Técnicas

### Backend (.NET 8)
- **Entity Framework Core** con PostgreSQL
- **JWT Authentication** stateless
- **CORS** configurado para desarrollo
- **Swagger/OpenAPI** para documentación
- **Validaciones** robustas en DTOs
- **Arquitectura limpia** con separación de responsabilidades
- **Manejo de fechas/horas** con TimeZone
- **Validación de conflictos** de reservas
- **Simulación de pagos** integrada

### Frontend (Angular 18)
- **Standalone Components** modernos
- **Reactive Forms** con validaciones
- **RxJS** para manejo de estado
- **Font Awesome** para iconografía
- **CSS moderno** con variables y gradientes
- **TypeScript** estricto para type safety
- **Interceptores HTTP** para autenticación
- **Guards de navegación** para rutas protegidas

## 🚨 Notas Importantes

### Sistema de Reservas
- Las reservas se pueden cancelar hasta **2 horas antes** de la hora de inicio
- El sistema valida automáticamente conflictos de horario
- Los pagos son **simulados** para propósitos de demostración
- Las horas disponibles van de **6:00 AM a 10:00 PM**
- Se permite reservar con **bloques de 1 hora mínimo**

### Seguridad
- Todas las rutas de reservas requieren **autenticación JWT**
- Los usuarios solo pueden ver y gestionar **sus propias reservas**
- Las contraseñas se encriptan con **BCrypt**
- Los tokens JWT expiran según configuración

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Para contribuir:

1. Fork el proyecto
2. Crea una rama feature (`git checkout -b feature/NuevaFuncionalidad`)
3. Commit tus cambios (`git commit -m 'Agregar nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/NuevaFuncionalidad`)
5. Abre un Pull Request

### Guías de Contribución
- Seguir las convenciones de naming establecidas
- Incluir tests para nuevas funcionalidades
- Actualizar documentación cuando sea necesario
- Mantener el estilo de código consistente
- Usar migraciones de EF Core para cambios de BD

## 👨‍💻 Autor

**Juan Carlos Salazar**
- GitHub: [@juancsalazaro](https://github.com/juancsalazaro)
- LinkedIn: [juan-camilo-salazar-osorio](https://www.linkedin.com/in/juan-camilo-salazar-osorio/)

---

⭐ Si te gusta este proyecto, ¡dale una estrella en GitHub!

🚀 **Estado del Proyecto**: Activamente desarrollado | **Versión**: 2.0.0 | **Última actualización**: Junio 2025
