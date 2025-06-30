# Sistema de Reservas para Canchas Deportivas 🏆

## 📋 Descripción

Este proyecto permite a los usuarios registrarse, iniciar sesión y gestionar reservas de canchas deportivas de manera intuitiva. El sistema cuenta con una arquitectura robusta dividida en backend y frontend, ofreciendo una experiencia de usuario fluida y moderna con **sistema de reservas completo** y **manejo de roles diferenciados**.

## 🚀 Tecnologías Utilizadas

### Backend
- **.NET 8** - Framework principal
- **PostgreSQL** - Base de datos principal
- **Entity Framework Core** - ORM
- **JWT** - Autenticación y autorización con roles
- **BCrypt** - Encriptación de contraseñas
- **Swagger** - Documentación de API
- **Sistema de Email** - Confirmaciones automáticas

### Frontend
- **Angular 18** - Framework de frontend
- **TypeScript** - Lenguaje de programación
- **Bootstrap** - Framework CSS
- **Font Awesome** - Iconografía
- **Reactive Forms** - Manejo de formularios
- **JWT Decoder** - Manejo de roles y tokens

## 👥 Sistema de Roles

El sistema cuenta con **roles diferenciados** que proporcionan diferentes niveles de acceso:

### 🔑 Roles Disponibles

| Rol | Descripción | Permisos |
|-----|-------------|----------|
| **Cliente** | Usuario estándar que puede reservar canchas | ✅ Ver canchas<br>✅ Hacer reservas<br>✅ Gestionar sus reservas<br>❌ Crear canchas |
| **Administrador** | Usuario con permisos de gestión de canchas | ✅ Ver canchas<br>✅ Crear canchas<br>✅ Gestionar sistema<br>❌ Hacer reservas |
| **Empleado** | Usuario con permisos limitados (futuro) | ✅ Ver canchas<br>❌ Crear canchas<br>❌ Hacer reservas |

### 🛡️ Protección de Rutas

- **Guards de autenticación**: Todas las rutas principales requieren login
- **Guards de roles**: Rutas específicas protegidas por rol:
  - `/mis-reservas` → Solo **Clientes**
  - `/crear-cancha` → Solo **Administradores**
- **Interfaz adaptativa**: Los botones y opciones se muestran según el rol del usuario

## 📁 Estructura del Proyecto

```
SistemaDeReservas-CanchasDeportivas/
├── BackEnd/
│   ├── Controllers/
│   │   ├── AuthController.cs              # ✅ CON ROLES
│   │   ├── CanchasController.cs
│   │   └── ReservasController.cs          # ✅ NUEVO
│   ├── Data/
│   │   └── AppDbContext.cs               # ✅ ACTUALIZADO CON ROLES
│   ├── Models/
│   │   ├── User.cs                       # ✅ CON CAMPO ROL
│   │   ├── Cancha.cs
│   │   └── Reserva.cs                    # ✅ NUEVO
│   ├── Dtos/
│   │   ├── UserDto.cs                    # ✅ CON ROL
│   │   ├── CanchaDto.cs
│   │   ├── CanchaResponseDto.cs
│   │   ├── ReservaDto.cs                 # ✅ NUEVO
│   │   ├── ReservaResponseDto.cs         # ✅ NUEVO
│   │   ├── DisponibilidadResponseDto.cs  # ✅ NUEVO
│   │   └── PagoSimuladoDto.cs           # ✅ NUEVO
│   ├── Services/
│   │   └── EmailService.cs               # ✅ NUEVO - ENVÍO DE CORREOS
│   ├── Guards/
│   │   └── RoleAttribute.cs              # ✅ NUEVO - AUTORIZACIÓN POR ROLES
│   ├── Enums/
│   │   └── UserRole.cs                   # ✅ NUEVO - ENUM DE ROLES
│   ├── Migrations/
│   └── Program.cs
├── FrontEnd/
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/
│   │   │   │   ├── auth.guard.ts
│   │   │   │   ├── role.guard.ts         # ✅ NUEVO - GUARD DE ROLES
│   │   │   │   ├── auth.interceptor.ts
│   │   │   │   ├── auth.service.ts       # ✅ ACTUALIZADO CON ROLES
│   │   │   │   └── jwt.service.ts        # ✅ NUEVO - DECODIFICACIÓN JWT
│   │   │   ├── features/
│   │   │   │   ├── login/
│   │   │   │   ├── register/             # ✅ CON SELECCIÓN DE ROL
│   │   │   │   ├── dashboard/            # ✅ INTERFAZ ADAPTATIVA POR ROL
│   │   │   │   └── reservas/             # ✅ NUEVO
│   │   │   ├── models/
│   │   │   │   ├── user.dto.ts           # ✅ CON ROL Y ENUM
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

3. **Configurar cadena de conexión y servicios**
   
   Actualizar `BackEnd/appsettings.json`:
   ```json
   {
     \"ConnectionStrings\": {
       \"DefaultConnection\": \"Host=localhost;Database=ReservasCanchas;Username=postgres;Password=tu_password;Port=5432\"
     },
     \"Jwt\": {
       \"Key\": \"TuClaveSecretaMuySeguraAqui123456789\"
     },
     \"EmailSettings\": {
       \"SmtpServer\": \"smtp.gmail.com\",
       \"SmtpPort\": 587,
       \"SenderEmail\": \"tu-email@gmail.com\",
       \"SenderPassword\": \"tu-app-password\",
       \"EnableSsl\": true
     }
   }
   ```

4. **Instalar dependencias y ejecutar migraciones**
   ```bash
   cd BackEnd
   dotnet restore
   dotnet ef migrations add AddRolesToUsers
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

| Método | Endpoint | Descripción | Roles |
|--------|----------|-------------|-------|
| POST | `/api/v1/auth/register` | Registrar nuevo usuario con rol | Público |
| POST | `/api/v1/auth/login` | Iniciar sesión (retorna rol en JWT) | Público |

### Canchas

| Método | Endpoint | Descripción | Roles |
|--------|----------|-------------|-------|
| GET | `/api/v1/canchas` | Obtener todas las canchas con filtros | Todos |
| GET | `/api/v1/canchas/{id}` | Obtener cancha por ID | Todos |
| POST | `/api/v1/canchas` | Crear nueva cancha | **Solo Administrador** |
| GET | `/api/v1/canchas/tipos-deporte` | Obtener tipos de deportes disponibles | Todos |

### 🆕 Reservas

| Método | Endpoint | Descripción | Roles |
|--------|----------|-------------|-------|
| POST | `/api/v1/reservas` | Crear nueva reserva + envío de email | **Solo Cliente** |
| GET | `/api/v1/reservas/mis-reservas` | Obtener reservas del usuario autenticado | **Solo Cliente** |
| GET | `/api/v1/reservas/{id}` | Obtener reserva específica por ID | **Solo Cliente** |
| GET | `/api/v1/reservas/disponibilidad` | Consultar disponibilidad de cancha por fecha | Todos |
| PATCH | `/api/v1/reservas/{id}/cancelar` | Cancelar una reserva | **Solo Cliente** |

### Ejemplo de uso

**Registro con rol:**
```json
POST /api/v1/auth/register
{
  \"username\": \"cliente123\",
  \"password\": \"contraseña123\",
  \"rol\": \"Cliente\"
}
```

**Login (respuesta incluye rol):**
```json
POST /api/v1/auth/login
{
  \"username\": \"cliente123\",
  \"password\": \"contraseña123\"
}

// Respuesta:
{
  \"token\": \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\",
  \"user\": {
    \"id\": 1,
    \"username\": \"cliente123\",
    \"rol\": \"Cliente\"
  }
}
```

**🆕 Crear reserva (con envío automático de email):**
```json
POST /api/v1/reservas
Authorization: Bearer {token}
{
  \"canchaId\": 1,
  \"fechaReserva\": \"2024-07-15T00:00:00\",
  \"horaInicio\": \"2024-07-15T14:00:00\",
  \"horaFin\": \"2024-07-15T16:00:00\",
  \"nombreCliente\": \"Juan Pérez\",
  \"emailCliente\": \"juan@email.com\",
  \"telefonoCliente\": \"123456789\",
  \"observaciones\": \"Reserva para partido amistoso\",
  \"datosPago\": {
    \"tipoTarjeta\": \"Visa\",
    \"numeroTarjeta\": \"4111 1111 1111 1111\",
    \"nombreTarjeta\": \"Juan Pérez\",
    \"fechaVencimiento\": \"12/25\",
    \"cvv\": \"123\"
  }
}
```

## 🎯 Funcionalidades Implementadas

### ✅ Autenticación y Seguridad con Roles
- **Registro de usuarios** con selección de rol
- **Inicio de sesión** con JWT que incluye información de rol
- **Encriptación de contraseñas** con BCrypt
- **Guards de autenticación y roles** para protección granular
- **Interfaz adaptativa** según permisos del usuario
- **Decodificación JWT** en frontend para manejo de roles

### ✅ Sistema de Roles Diferenciado
- **Clientes**: Pueden hacer reservas pero no crear canchas
- **Administradores**: Pueden crear canchas pero no hacer reservas
- **Visualización del rol** en la interfaz de usuario
- **Protección de rutas** basada en roles
- **Mensajes de error** informativos para accesos no autorizados

### ✅ Gestión de Canchas
- **CRUD completo** de canchas deportivas
- **Filtros avanzados** por deporte, precio y disponibilidad
- **Búsqueda por ubicación** integrada
- **Categorización** por tipos de deporte
- **Sistema de calificaciones** y reseñas
- **Creación restringida** solo para administradores

### 🆕 ✅ Sistema de Reservas con Notificaciones
- **Creación de reservas** con validación de disponibilidad
- **📧 Envío automático de correo** de confirmación al crear reserva
- **Gestión de horarios** por bloques de tiempo
- **Verificación de conflictos** de reservas
- **Consulta de disponibilidad** en tiempo real
- **Cancelación de reservas** con restricciones de tiempo
- **Historial de reservas** por usuario (solo clientes)
- **Simulación de pagos** con múltiples métodos
- **Cálculo automático** de precios por hora
- **Estados de reserva** (Confirmada, Cancelada, Completada)

### ✅ Dashboard Adaptativo por Rol
- **Interfaz diferenciada** según el rol del usuario
- **Botones contextuales** que aparecen según permisos
- **Información del usuario y rol** visible en header
- **Navegación intuitiva** adaptada a funcionalidades disponibles
- **Iconos distintivos** para cada tipo de rol

### 📧 Sistema de Notificaciones por Email
- **Confirmación automática** al crear una reserva
- **Detalles completos** de la reserva en el correo
- **Información de la cancha** y horarios reservados
- **Datos de contacto** para soporte
- **Diseño profesional** del template de correo

## 🗄️ Modelo de Base de Datos

### Tabla: Users (✅ Actualizada)
```sql
- Id (int, PK)
- Username (varchar, unique)
- PasswordHash (varchar)
- Rol (varchar) -- 'Cliente', 'Administrador', 'Empleado'
- CreatedAt (timestamp)
- UpdatedAt (timestamp)
- IsActive (boolean)
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

### Dashboard con Roles
*Interfaz principal que se adapta según el rol del usuario (Cliente/Administrador)*

### Sistema de Registro con Roles
*Formulario de registro que permite seleccionar el rol del usuario*

### Gestión de Reservas (Solo Clientes)
*Panel exclusivo para clientes para gestionar sus reservas*

### Creación de Canchas (Solo Administradores)
*Formulario exclusivo para administradores para crear nuevas canchas*

### Sistema de Email
*Correo de confirmación automático enviado tras cada reserva*

## 🔍 Características Técnicas

### Backend (.NET 8)
- **Entity Framework Core** con PostgreSQL
- **JWT Authentication** con claims de roles
- **Autorización basada en roles** con atributos personalizados
- **Sistema de email SMTP** para notificaciones
- **CORS** configurado para desarrollo
- **Swagger/OpenAPI** con documentación de roles
- **Validaciones** robustas en DTOs
- **Enum de roles** para type safety
- **Migración de base de datos** para campo rol

### Frontend (Angular 18)
- **JWT Service** para decodificación de tokens
- **Role Guards** para protección de rutas
- **Servicios de autenticación** con manejo de roles
- **Interfaz adaptativa** que responde a permisos
- **Componentes standalone** modernos
- **Reactive Forms** con validaciones
- **TypeScript** estricto para type safety

## 🚨 Notas Importantes

### Sistema de Roles
- Los **Clientes** pueden reservar pero no crear canchas
- Los **Administradores** pueden crear canchas pero no reservar
- El rol se asigna durante el registro y se valida en cada request
- Las rutas están protegidas tanto en frontend como backend
- La interfaz se adapta automáticamente según el rol del usuario

### Sistema de Reservas y Notificaciones
- Las reservas se pueden cancelar hasta **2 horas antes** de la hora de inicio
- **Correo de confirmación automático** se envía al email del cliente
- El sistema valida automáticamente conflictos de horario
- Los pagos son **simulados** para propósitos de demostración
- Las horas disponibles van de **6:00 AM a 10:00 PM**
- Se permite reservar con **bloques de 1 hora mínimo**

### Seguridad
- Todas las rutas requieren **autenticación JWT**
- **Autorización por roles** implementada en backend y frontend
- Los usuarios solo pueden acceder a funciones según su rol
- Las contraseñas se encriptan con **BCrypt**
- Los tokens JWT incluyen claims de rol para validación

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
- Respetar el sistema de roles implementado
- Actualizar documentación cuando sea necesario
- Mantener el estilo de código consistente
- Usar migraciones de EF Core para cambios de BD

## 👨‍💻 Autor

**Juan Camilo Salazar**  
**Hanner Obando**

- GitHub: [@juancsalazaro](https://github.com/juancsalazaro)
- LinkedIn: [juan-camilo-salazar-osorio](https://www.linkedin.com/in/juan-camilo-salazar-osorio/)

---

⭐ Si te gusta este proyecto, ¡dale una estrella en GitHub!

🚀 **Estado del Proyecto**: Activamente desarrollado | **Versión**: 2.1.0 | **Última actualización**: Junio 2025

### 🔄 Changelog v2.1.0
- ✅ **Sistema de roles** completo implementado
- ✅ **Guards de autorización** por rol en frontend y backend
- ✅ **Interfaz adaptativa** según permisos de usuario
- ✅ **Sistema de email** para confirmaciones de reserva
- ✅ **JWT con claims de rol** para seguridad granular
- ✅ **Migración de base de datos** para soporte de roles
