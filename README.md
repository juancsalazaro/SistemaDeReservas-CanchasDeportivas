# Sistema de Reservas de Canchas Deportivas 🏆⚽

Un sistema web moderno para la gestión y reserva de canchas deportivas, desarrollado con tecnologías de vanguardia.

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
│   │   └── AuthController.cs
│   ├── Data/
│   │   └── AppDbContext.cs
│   ├── Models/
│   │   └── User.cs
│   ├── Dtos/
│   │   └── UserDto.cs
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
│   │   │   │   └── register/
│   │   │   ├── models/
│   │   │   │   └── user.dto.ts
│   │   │   └── services/
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
   CREATE DATABASE reservas_canchas;
   
   -- Crear usuario (opcional)
   CREATE USER reservas_user WITH PASSWORD 'tu_password';
   GRANT ALL PRIVILEGES ON DATABASE reservas_canchas TO reservas_user;
   ```

3. **Configurar cadena de conexión**
   
   Actualizar `BackEnd/appsettings.json`:
   ```json
   {
     \"ConnectionStrings\": {
       \"DefaultConnection\": \"Host=localhost;Database=reservas_canchas;Username=postgres;Password=tu_password;Port=5432\"
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

## 🎯 Funcionalidades Implementadas

- ✅ **Registro de usuarios** con validaciones
- ✅ **Inicio de sesión** con JWT
- ✅ **Validación de formularios** en tiempo real
- ✅ **Diseño responsive** para todos los dispositivos
- ✅ **Encriptación de contraseñas** con BCrypt
- ✅ **Manejo de errores** y feedback al usuario
- ✅ **Interfaz moderna** con efectos y animaciones

## 🔮 Funcionalidades Futuras

- 🔄 Gestión de canchas deportivas
- 📅 Sistema de reservas por fecha/hora
- 👥 Panel de administración
- 📊 Reportes y estadísticas
- 💳 Integración de pagos
- 📱 Aplicación móvil
- 🔔 Sistema de notificaciones

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
```

### Frontend
```bash
# Instalar dependencias
npm install

# Ejecutar en desarrollo
ng serve

# Compilar para producción
ng build --prod

# Ejecutar tests
ng test

# Generar componente
ng generate component nombre-componente
```

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Para contribuir:

1. Fork el proyecto
2. Crea una rama feature (`git checkout -b feature/NuevaFuncionalidad`)
3. Commit tus cambios (`git commit -m 'Agregar nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/NuevaFuncionalidad`)
5. Abre un Pull Request

## 👨‍💻 Autor

**Juan Carlos Salazar**
- GitHub: [@juancsalazaro](https://github.com/juancsalazaro)
- LinkedIn: [juan-camilo-salazar-osorio]([https://linkedin.com/in/tu-perfil](https://www.linkedin.com/in/juan-camilo-salazar-osorio/))

---

⭐ Si te gusta este proyecto, ¡dale una estrella!
