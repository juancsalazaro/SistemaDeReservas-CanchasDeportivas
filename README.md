## 📋 Descripción

Este proyecto permite a los usuarios registrarse, iniciar sesión y gestionar reservas de canchas deportivas de manera intuitiva. El sistema cuenta con una arquitectura robusta dividida en backend y frontend, ofreciendo una experiencia de usuario fluida y moderna.

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
│   │   └── CanchasController.cs
│   ├── Data/
│   │   └── AppDbContext.cs
│   ├── Models/
│   │   ├── User.cs
│   │   └── Cancha.cs
│   ├── Dtos/
│   │   ├── UserDto.cs
│   │   ├── CanchaDto.cs
│   │   └── CanchaResponseDto.cs
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
│   │   │   │   └── dashboard/
│   │   │   ├── models/
│   │   │   │   ├── user.dto.ts
│   │   │   │   └── cancha.dto.ts
│   │   │   └── services/
│   │   │       └── canchas.service.ts
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
     \"ConnectionStrings\": {
       \"DefaultConnection\": \"Host=localhost;Database=ReservasCanchas;Username=postgres;Password=tu_password;Port=5432\"
     },
     \"Jwt\": {
       \"Key\": \"TuClaveSecretaMuySeguraAqui123456789\"
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

### Ejemplo de uso

**Registro:**
```json
POST /api/v1/auth/register
{
  \"username\": \"usuario123\",
  \"password\": \"contraseña123\"
}
```

**Login:**
```json
POST /api/v1/auth/login
{
  \"username\": \"usuario123\",
  \"password\": \"contraseña123\"
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
  \"nombre\": \"Cancha El Estadio\",
  \"descripcion\": \"Cancha de fútbol profesional con césped sintético\",
  \"tipoDeporte\": \"Futbol\",
  \"ubicacion\": \"Estadio La Nubia, Manizales\",
  \"precioPorHora\": 80000,
  \"amenidades\": \"[\\\"Vestuarios\\\", \\\"Estacionamiento\\\", \\\"Iluminación\\\"]\"
}
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

## 🔮 Funcionalidades Futuras

- 📅 **Sistema de reservas** por fecha/hora
- 💳 **Pasarela de pagos** integrada
- 👥 **Panel de administración** avanzado
- 📊 **Dashboard de estadísticas** y reportes
- 🔔 **Sistema de notificaciones** en tiempo real
- 📱 **Aplicación móvil** nativa
- 🗓️ **Calendario de disponibilidad** interactivo
- ⭐ **Sistema de reseñas** y comentarios
- 🏆 **Programa de fidelización** para usuarios
- 📍 **Integración con mapas** para ubicaciones

## 🛠️ Comandos Útiles

### Backend
```bash
# Restaurar paquetes
dotnet restore

# Crear nueva migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones
dotnet ef database update

# Ejecutar en desarrollo
dotnet run

# Compilar para producción
dotnet publish -c Release

# Ver ayuda de EF Core
dotnet ef --help
```

### Frontend
```bash
# Instalar dependencias
npm install

# Ejecutar en desarrollo
ng serve

# Compilar para producción
ng build --configuration production

# Ejecutar tests
ng test

# Generar componente
ng generate component features/nombre-componente

# Generar servicio
ng generate service services/nombre-servicio

# Linting del código
ng lint
```

## 🎨 Capturas de Pantalla

### Dashboard Principal
*Interfaz principal con búsqueda avanzada y grid de canchas deportivas*

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

### Frontend (Angular 18)
- **Standalone Components** modernos
- **Reactive Forms** con validaciones
- **RxJS** para manejo de estado
- **Font Awesome** para iconografía
- **CSS moderno** con variables y gradientes
- **TypeScript** estricto para type safety

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

## 👨‍💻 Autor

**Juan Carlos Salazar**
- GitHub: [@juancsalazaro](https://github.com/juancsalazaro)
- LinkedIn: [juan-camilo-salazar-osorio](https://www.linkedin.com/in/juan-camilo-salazar-osorio/)

---

⭐ Si te gusta este proyecto, ¡dale una estrella en GitHub!

🚀 **Estado del Proyecto**: Activamente desarrollado | **Versión**: 1.0.0 | **Última actualización**: Junio 2025
`
