# 📚 DOCUMENTACIÓN COMPLETA DEL PROYECTO - Sistema de Reserva de Cine

## 📋 Tabla de Contenidos
1. [Visión General](#visión-general)
2. [Arquitectura del Sistema](#arquitectura-del-sistema)
3. [Estructura del Proyecto](#estructura-del-proyecto)
4. [Modelos de Datos](#1-modelos-de-datos)
5. [Servicios](#servicios)
6. [Ventanas de la Aplicación](#ventanas-de-la-aplicación)
7. [Flujo de Navegación](#flujo-de-navegación)

---

## 🎯 Visión General

### Descripción del Proyecto
Sistema de reserva de cine desarrollado en **WPF (Windows Presentation Foundation)** con **.NET 10.0**, que permite a los usuarios ver la cartelera de películas, seleccionar sesiones, reservar butacas y gestionar su perfil.

### Características Principales
- ✅ Visualización de cartelera de películas activas
- ✅ Selección de sesiones por fecha
- ✅ Selección visual de butacas con efecto de perspectiva
- ✅ **Sistema de pago simulado con múltiples métodos**
- ✅ Sistema de autenticación de usuarios
- ✅ Registro de nuevos usuarios
- ✅ Gestión de perfil y cambio de contraseña
- ✅ Historial de reservas
- ✅ Generación de códigos de reserva únicos
- ✅ Manejo de estados de butacas (disponible, ocupada, seleccionada)
- ✅ Soporte para diferentes tipos de butacas (Normal, VIP, Discapacitado)

### Tecnologías Utilizadas
- **Framework**: .NET 10.0
- **UI Framework**: WPF (Windows Presentation Foundation)
- **Base de Datos**: MySQL
- **Patrón de Arquitectura**: MVVM (Model-View-ViewModel) parcial
- **Programación Asíncrona**: async/await
- **ORM**: ADO.NET con MySql.Data

---

## 🏗️ Arquitectura del Sistema

### Patrón de Diseño
El proyecto sigue una arquitectura en capas:

```
┌────────────────────────────────────────┐
│         Capa de Presentación           │
│     (Ventanas XAML + Code-behind)      │
├────────────────────────────────────────┤
│           Capa de Servicios            │
│  (ServicioBaseDeDatos, ServicioSesion) │
├────────────────────────────────────────┤
│             Capa de Modelos            │
│     (Entidades: Usuario, Pelicula,     │
│       Sesion, Butaca, Reserva)         │
├────────────────────────────────────────┤
│             Capa de Datos              │
│         (Base de Datos MySQL)          │
└────────────────────────────────────────┘
```

### Principios Aplicados
- **Separación de Responsabilidades**: Cada capa tiene una responsabilidad específica
- **Singleton Pattern**: ServicioSesion implementa el patrón Singleton
- **Programación Asíncrona**: Operaciones de base de datos ejecutadas de forma asíncrona
- **Manejo de Errores**: Try-catch en operaciones críticas con mensajes al usuario

---

## 📁 Estructura del Proyecto

### Vista de Árbol Completa

```
🎬 Cine_app/
│
├── 📂 Modelos/                         # Entidades del dominio
│   ├── 📄 Usuario.cs                   # Modelo de datos de usuario
│   │   ├── ✓ Propiedades: Id, Nombre, Apellidos, Email, Password
│   │   ├── ✓ Propiedad computada: NombreCompleto
│   │   └── ✓ Validación: Activo, FechaRegistro
│   │
│   ├── 📄 Pelicula.cs                  # Modelo de datos de película
│   │   ├── ✓ Propiedades: Id, Titulo, Descripcion, Director
│   │   ├── ✓ Información: Duracion, Genero, FechaEstreno
│   │   ├── ✓ Multimedia: ImagenUrl, Calificacion
│   │   └── ✓ Estado: Activa
│   │
│   ├── 📄 Sesion.cs                    # Modelo de sesión/función
│   │   ├── ✓ Clase Sesion
│   │   │   ├── → Propiedades: Id, PeliculaId, SalaId
│   │   │   ├── → FechaHora, Precio, Activa
│   │   │   ├── → Navegación: Pelicula, Sala
│   │   │   └── → Formato: FechaHoraFormateada
│   │   │
│   │   └── ✓ Clase Sala
│   │       ├── → Id, Nombre
│   │       ├── → Filas, ColumnasPerFila
│   │       └── → CapacidadTotal (computed)
│   │
│   └── 📄 Butaca.cs                    # Modelos de butacas y reservas
│       ├── ✓ Clase Butaca
│       │   ├── → Id, SalaId, Fila, Columna
│       │   ├── → Tipo (Normal/VIP/Discapacitado)
│       │   ├── → Activa
│       │   └── → Identificador (ej: "A1", "B5")
│       │
│       ├── ✓ Clase Reserva
│       │   ├── → Id, UsuarioId, SesionId
│       │   ├── → FechaReserva, Total, Estado
│       │   ├── → CodigoReserva (único)
│       │   └── → Navegación: Usuario, Sesion, Butacas
│       │
│       ├── ✓ Clase ReservaButaca
│       │   ├── → Id, ReservaId, ButacaId, SesionId
│       │   └── → Navegación: Butaca
│       │
│       └── ✓ Clase ReservaViewModel
│           └── → Para binding en UI
│
├── 📂 Servicios/                       # Lógica de negocio
│   ├── 📄 ServicioBaseDeDatos.cs       # Acceso y operaciones de BD
│   │   ├── 🔧 Constructor
│   │   │   └── → Carga connectionString desde .env
│   │   │
│   │   ├── 🎬 Métodos de Películas
│   │   │   └── → ObtenerPeliculasActivasAsync()
│   │   │
│   │   ├── 📅 Métodos de Sesiones
│   │   │   └── → ObtenerSesionesPorPeliculaAsync()
│   │   │
│   │   ├── 💺 Métodos de Butacas
│   │   │   ├── → ObtenerButacasPorSalaAsync()
│   │   │   └── → ObtenerButacasReservadasAsync()
│   │   │
│   │   ├── 👤 Métodos de Usuarios
│   │   │   ├── → ValidarUsuarioAsync()
│   │   │   ├── → ExisteUsuarioAsync()
│   │   │   ├── → RegistrarUsuarioAsync()
│   │   │   └── → ActualizarPasswordAsync()
│   │   │
│   │   └── 🎟️ Métodos de Reservas
│   │       ├── → ObtenerReservasPorUsuarioAsync()
│   │       ├── → ObtenerButacasDeReservaAsync() [privado]
│   │       └── → CrearReservaAsync() [con transacción]
│   │
│   └── 📄 ServicioSesion.cs            # Gestión de sesión (Singleton)
│       ├── 🔒 Propiedades
│       │   ├── → Instance (Singleton)
│       │   ├── → UsuarioActual
│       │   └── → EstaAutenticado
│       │
│       ├── 📡 Eventos
│       │   ├── → SesionIniciada
│       │   └── → SesionCerrada
│       │
│       └── 🔧 Métodos
│           ├── → IniciarSesion(Usuario)
│           └── → CerrarSesion()
│
├── 📂 Ventanas/                        # Interfaz de usuario (Views)
│   │
│   ├── 🎬 CarteleraWindow.xaml[.cs]    # Ventana principal
│   │   ├── 📋 Responsabilidades
│   │   │   ├── → Mostrar cartelera de películas
│   │   │   ├── → Gestión de sesión de usuario
│   │   │   └── → Navegación a otras ventanas
│   │   │
│   │   └── 🔧 Métodos principales
│   │       ├── → CarteleraWindow_Loaded()
│   │       ├── → ActualizarEstadoUsuario()
│   │       ├── → CargarPeliculas()
│   │       ├── → Pelicula_Click()
│   │       ├── → BtnVerHorarios_Click()
│   │       ├── → BtnCuentaAccion_Click()
│   │       └── → BtnPerfilUsuario_Click()
│   │
│   ├── 🔐 LoginWindow.xaml[.cs]        # Inicio de sesión
│   │   ├── 📋 Responsabilidades
│   │   │   ├── → Validar credenciales
│   │   │   ├── → Autenticación de usuarios
│   │   │   └── → Navegación a registro
│   │   │
│   │   └── 🔧 Métodos principales
│   │       ├── → BtnLogin_Click()
│   │       ├── → BtnRegistro_Click()
│   │       ├── → BtnInvitado_Click()
│   │       ├── → MostrarError()
│   │       └── → TxtPassword_KeyDown()
│   │
│   ├── 📝 RegistroWindow.xaml[.cs]     # Registro de usuarios
│   │   ├── 📋 Responsabilidades
│   │   │   ├── → Validación de formulario
│   │   │   ├── → Registro de nuevos usuarios
│   │   │   └── → Verificación de email único
│   │   │
│   │   └── 🔧 Métodos principales
│   │       ├── → BtnRegistrar_Click()
│   │       ├── → ValidarCampos()
│   │       │   ├── ✓ Valida nombre (min 2 chars)
│   │       │   ├── ✓ Valida apellidos (min 2 chars)
│   │       │   ├── ✓ Valida email (regex)
│   │       │   ├── ✓ Valida teléfono (opcional, min 9)
│   │       │   ├── ✓ Valida password (min 6 chars)
│   │       │   └── ✓ Confirma password
│   │       ├── → EsEmailValido()
│   │       ├── → MostrarError()
│   │       ├── → MostrarExito()
│   │       └── → BtnCancelar_Click()
│   │
│   ├── 📅 SeleccionSesionWindow.xaml[.cs]  # Selección de horarios
│   │   ├── 📋 Responsabilidades
│   │   │   ├── → Mostrar info de película
│   │   │   ├── → Calendario de fechas
│   │   │   ├── → Listar sesiones disponibles
│   │   │   └── → Verificar autenticación
│   │   │
│   │   └── 🔧 Métodos principales
│   │       ├── → CargarInfoPelicula()
│   │       ├── → Calendario_SelectedDatesChanged()
│   │       ├── → CargarSesiones()
│   │       ├── → BtnSeleccionarSesion_Click()
│   │       ├── → AbrirSeleccionButacas()
│   │       └── → BtnCerrar_Click()
│   │
│   ├── 💺 SeleccionButacasWindow.xaml[.cs]  # Selección de butacas
│   │   ├── 📋 Responsabilidades
│   │   │   ├── → Visualización 3D de sala
│   │   │   ├── → Gestión de butacas (disponible/ocupada/seleccionada)
│   │   │   ├── → Cálculo de total en tiempo real
│   │   │   └── → Navegación a ventana de pago
│   │   │
│   │   ├── 🎨 Características visuales
│   │   │   ├── ✓ Efecto de perspectiva
│   │   │   ├── ✓ Tamaño dinámico por fila
│   │   │   ├── ✓ Espaciado lateral (cono)
│   │   │   └── ✓ Estilos por tipo de butaca
│   │   │
│   │   └── 🔧 Métodos principales
│   │       ├── → CargarInformacion()
│   │       ├── → CargarButacas()
│   │       ├── → CrearVisualizacionButacas()
│   │       ├── → CrearBotonButaca()
│   │       ├── → BtnButaca_Click()
│   │       ├── → ActualizarResumen()
│   │       ├── → BtnConfirmarReserva_Click() [Abre PagoWindow]
│   │       └── → ProcesarReserva() [Después del pago]
│   │
│   ├── 💳 PagoWindow.xaml[.cs]         # **NUEVA** Ventana de pago
│   │   ├── 📋 Responsabilidades
│   │   │   ├── → Selección de método de pago
│   │   │   ├── → Validación de datos de pago
│   │   │   ├── → Simulación de procesamiento
│   │   │   └── → Confirmación de pago exitoso
│   │   │
│   │   ├── 💳 Métodos de Pago
│   │   │   ├── → Tarjeta de Crédito/Débito
│   │   │   ├── → Bizum
│   │   │   └── → PayPal
│   │   │
│   │   ├── 🔒 Validaciones por Método
│   │   │   ├── → Tarjeta:
│   │   │   │   ├── ✓ Número (16 dígitos)
│   │   │   │   ├── ✓ Titular (min 3 caracteres)
│   │   │   │   ├── ✓ Fecha expiración (MM/AA, no vencida)
│   │   │   │   └── ✓ CVV (3 o 4 dígitos)
│   │   │   ├── → Bizum:
│   │   │   │   └── ✓ Teléfono (9 dígitos, empieza por 6, 7 o 9)
│   │   │   └── → PayPal:
│   │   │       ├── ✓ Email (formato válido)
│   │   │       └── ✓ Contraseña (min 6 caracteres)
│   │   │
│   │   └── 🔧 Métodos principales
│   │       ├── → BtnMetodoTarjeta_Click()
│   │       ├── → BtnMetodoBizum_Click()
│   │       ├── → BtnMetodoPaypal_Click()
│   │       ├── → SeleccionarMetodoPago()
│   │       ├── → MostrarFormulario()
│   │       ├── → ResaltarBoton()
│   │       ├── → ValidarFormularioTarjeta()
│   │       ├── → ValidarFormularioBizum()
│   │       ├── → ValidarFormularioPaypal()
│   │       ├── → BtnPagar_Click()
│   │       ├── → BtnCancelar_Click()
│   │       ├── → MostrarError()
│   │       └── → OcultarError()
│   │
│   └── 👤 PerfilUsuarioWindow.xaml[.cs]    # Perfil y reservas
│       ├── 📋 Responsabilidades
│       │   ├── → Mostrar información de usuario
│       │   ├── → Cambiar contraseña
│       │   ├── → Historial de reservas
│       │   └── → Formato de datos de reserva
│       │
│       ├── 📑 Tabs/Secciones
│       │   ├── ℹ️ Información Personal
│       │   │   ├── → Datos del usuario
│       │   │   └── → Cambio de password
│       │   │
│       │   └── 🎟️ Mis Reservas
│       │       ├── → Lista de reservas activas
│       │       └── → Detalle por reserva
│       │
│       └── 🔧 Métodos principales
│           ├── → CargarInformacionUsuario()
│           ├── → BtnMenuInformacion_Click()
│           ├── → BtnMenuReservas_Click()
│           ├── → CargarReservas()
│           ├── → BtnCambiarPassword_Click()
│           ├── → MostrarMensajePassword()
│           └── → BtnVolver_Click()
│
├── 📂 Database/                        # Scripts de base de datos
│   └── 📄 cinema_database_mysql.sql    # Script completo de creación
│       ├── 📊 Tablas
│       │   ├── → Usuarios
│       │   ├── → Peliculas
│       │   ├── → Salas
│       │   ├── → Sesiones
│       │   ├── → Butacas
│       │   ├── → Reservas
│       │   └── → ReservasButacas
│       │
│       └── 🔗 Relaciones/Foreign Keys
│           ├── → Sesiones ← Peliculas
│           ├── → Sesiones ← Salas
│           ├── → Butacas ← Salas
│           ├── → Reservas ← Usuarios
│           ├── → Reservas ← Sesiones
│           ├── → ReservasButacas ← Reservas
│           ├── → ReservasButacas ← Butacas
│           └── → ReservasButacas ← Sesiones
│
├── 📂 obj/                             # Archivos temporales de compilación
│   └── 📂 Debug/
│       └── 📂 net10.0-windows/
│           ├── 📄 *.g.i.cs             # Archivos generados de XAML
│           ├── 📄 Cine_app.AssemblyInfo.cs
│           └── 📄 Cine_app.GlobalUsings.g.cs
│
├── 📄 App.xaml                         # Definición de recursos de aplicación
│   ├── 🎨 ResourceDictionaries
│   │   ├── → Estilos globales
│   │   ├── → Colores y brushes
│   │   ├── → Templates
│   │   └── → Estilos de butacas
│   │
│   └── 🔧 Configuración
│       └── → StartupUri="Ventanas/CarteleraWindow.xaml"
│
├── 📄 App.xaml.cs                      # Code-behind de aplicación
│   ├── 🚀 Application_Startup()
│   │   ├── → Configuración de cultura (es-ES)
│   │   └── → Apertura de CarteleraWindow
│   │
│   └── ⚠️ Manejadores de errores
│       ├── → App_DispatcherUnhandledException
│       └── → CurrentDomain_UnhandledException
│
├── 📄 Cine_app.csproj                  # Archivo de proyecto
│   ├── ⚙️ Configuración
│   │   ├── → TargetFramework: net10.0-windows
│   │   ├── → OutputType: WinExe
│   │   ├── → UseWPF: true
│   │   └── → Nullable: enable
│   │
│   ├── 📦 PackageReferences
│   │   ├── → MySql.Data (v9.5.0)
│   │   └── → DotNetEnv (v3.1.1)
│   │
│   └── 📁 ItemGroups
│       ├── → Page Include="App.xaml"
│       └── → None Update=".env"
│
├── 📄 .env                             # Variables de entorno
│   └── 🔐 DATABASE=server=...;database=...;user=...;password=...
│
└── 📄 .gitignore                       # Archivos ignorados por Git
    ├── → bin/
    ├── → obj/
    ├── → .env
    └── → *.user
```

### Descripción de Componentes por Carpeta

#### 📂 Modelos/ (Entidades del Dominio)
| Archivo | Responsabilidad | Clases Principales |
|---------|----------------|-------------------|
| `Usuario.cs` | Datos de usuario del sistema | Usuario |
| `Pelicula.cs` | Datos de películas en cartelera | Pelicula |
| `Sesion.cs` | Funciones y salas | Sesion, Sala |
| `Butaca.cs` | Butacas y reservas | Butaca, Reserva, ReservaButaca, ReservaViewModel |

#### 📂 Servicios/ (Lógica de Negocio)
| Archivo | Responsabilidad | Patrón |
|---------|----------------|--------|
| `ServicioBaseDeDatos.cs` | CRUD y consultas a MySQL | Repository |
| `ServicioSesion.cs` | Gestión de usuario autenticado | Singleton |

#### 📂 Ventanas/ (Interfaz de Usuario)
| Archivo | Ventana | Función Principal |
|---------|---------|-------------------|
| `CarteleraWindow` | Principal | Muestra películas disponibles |
| `LoginWindow` | Modal | Autenticación de usuarios |
| `RegistroWindow` | Modal | Registro de nuevos usuarios |
| `SeleccionSesionWindow` | Modal | Selección de fecha/hora |
| `SeleccionButacasWindow` | Modal | Selección de asientos |
| `PagoWindow` | Modal | Procesamiento de pago |
| `PerfilUsuarioWindow` | Modal | Perfil y reservas |

---

## 1. Modelos de Datos

### 1. **Usuario.cs**
Representa un usuario del sistema.

```csharp
public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Apellidos { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Telefono { get; set; }
    public DateTime FechaRegistro { get; set; }
    public bool Activo { get; set; }
    
    // Propiedad computada
    public string NombreCompleto => $"{Nombre} {Apellidos}";
}
```

**Propiedades:**
- `Id`: Identificador único del usuario
- `Nombre`: Nombre del usuario
- `Apellidos`: Apellidos del usuario
- `Email`: Correo electrónico (usado para login)
- `Password`: Contraseña (actualmente sin hash - mejorar en producción)
- `Telefono`: Teléfono opcional
- `FechaRegistro`: Fecha de creación de la cuenta
- `Activo`: Estado de la cuenta (activo/inactivo)
- `NombreCompleto`: Concatenación de nombre y apellidos

---

### 2. **Pelicula.cs**
Representa una película en cartelera.

```csharp
public class Pelicula
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string? Descripcion { get; set; }
    public string? Director { get; set; }
    public int? Duracion { get; set; }
    public string? Genero { get; set; }
    public DateTime? FechaEstreno { get; set; }
    public string? ImagenUrl { get; set; }
    public decimal? Calificacion { get; set; }
    public bool Activa { get; set; }
}
```

**Propiedades:**
- `Id`: Identificador único de la película
- `Titulo`: Título de la película
- `Descripcion`: Sinopsis o descripción
- `Director`: Nombre del director
- `Duracion`: Duración en minutos
- `Genero`: Género cinematográfico
- `FechaEstreno`: Fecha de estreno
- `ImagenUrl`: URL de la imagen/poster
- `Calificacion`: Calificación (1-10)
- `Activa`: Si está disponible en cartelera

---

### 3. **Sesion.cs**
Representa una sesión/función de una película.

```csharp
public class Sesion
{
    public int Id { get; set; }
    public int PeliculaId { get; set; }
    public int SalaId { get; set; }
    public DateTime FechaHora { get; set; }
    public decimal Precio { get; set; }
    public bool Activa { get; set; }
    
    // Propiedades de navegación
    public Pelicula? Pelicula { get; set; }
    public Sala? Sala { get; set; }
    
    // Propiedad computada
    public string FechaHoraFormateada => FechaHora.ToString("dd/MM/yyyy HH:mm");
}

public class Sala
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int Filas { get; set; }
    public int ColumnasPerFila { get; set; }
    public int CapacidadTotal => Filas * ColumnasPerFila;
}
```

**Sesion - Propiedades:**
- `Id`: Identificador de la sesión
- `PeliculaId`: FK a la película
- `SalaId`: FK a la sala
- `FechaHora`: Fecha y hora de la función
- `Precio`: Precio de la entrada
- `Activa`: Si la sesión está disponible
- `Pelicula`: Objeto película relacionado
- `Sala`: Objeto sala relacionada
- `FechaHoraFormateada`: Formato de fecha legible

**Sala - Propiedades:**
- `Id`: Identificador de la sala
- `Nombre`: Nombre de la sala
- `Filas`: Número de filas
- `ColumnasPerFila`: Butacas por fila
- `CapacidadTotal`: Cálculo de capacidad total

---

### 4. **Butaca.cs**
Contiene modelos relacionados con butacas y reservas.

```csharp
public class Butaca
{
    public int Id { get; set; }
    public int SalaId { get; set; }
    public int Fila { get; set; }
    public int Columna { get; set; }
    public string Tipo { get; set; } // Normal, VIP, Discapacitado
    public bool Activa { get; set; }
    
    // Propiedad computada
    public string Identificador => $"{(char)('A' + Fila - 1)}{Columna}";
}

public class Reserva
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int SesionId { get; set; }
    public DateTime FechaReserva { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } // Pendiente, Confirmada, Cancelada
    public string? CodigoReserva { get; set; }
    
    // Propiedades de navegación
    public Usuario? Usuario { get; set; }
    public Sesion? Sesion { get; set; }
    public List<ReservaButaca> Butacas { get; set; } = new();
}

public class ReservaButaca
{
    public int Id { get; set; }
    public int ReservaId { get; set; }
    public int ButacaId { get; set; }
    public int SesionId { get; set; }
    
    public Butaca? Butaca { get; set; }
}

public class ReservaViewModel
{
    public Sesion Sesion { get; set; } = new();
    public decimal Total { get; set; }
    public string CodigoReserva { get; set; } = string.Empty;
    public string Butacas { get; set; } = string.Empty;
}
```

**Butaca - Propiedades:**
- `Id`: Identificador de la butaca
- `SalaId`: FK a la sala
- `Fila`: Número de fila (1, 2, 3...)
- `Columna`: Número de columna
- `Tipo`: Tipo de butaca (Normal, VIP, Discapacitado)
- `Activa`: Si está disponible para reserva
- `Identificador`: Formato legible (ej: "A1", "B5")

**Reserva - Propiedades:**
- `Id`: Identificador de la reserva
- `UsuarioId`: FK al usuario que reserva
- `SesionId`: FK a la sesión
- `FechaReserva`: Fecha de la reserva
- `Total`: Monto total pagado
- `Estado`: Estado de la reserva
- `CodigoReserva`: Código único de reserva

**ReservaButaca:**
Tabla intermedia que relaciona reservas con butacas específicas.

**ReservaViewModel:**
Modelo de vista para mostrar reservas en la UI de manera formateada.

---

## Servicios

### 1. **ServicioBaseDeDatos.cs**
Servicio principal para todas las operaciones de base de datos.

#### Constructor
```csharp
public ServicioBaseDeDatos()
{
    DotNetEnv.Env.Load();
    connectionString = Environment.GetEnvironmentVariable("DATABASE")  string.Empty;
}
```
- Carga variables de entorno desde `.env`
- Obtiene la cadena de conexión a MySQL

---

#### Métodos de Películas

**`ObtenerPeliculasActivasAsync()`**
```csharp
public async Task<List<Pelicula>> ObtenerPeliculasActivasAsync()
```
- **Descripción**: Obtiene todas las películas activas en cartelera
- **Retorno**: Lista de películas ordenadas por fecha de estreno
- **Query**: `SELECT * FROM Peliculas WHERE Activa = 1 ORDER BY FechaEstreno DESC`
- **Uso**: Ventana de cartelera principal

---

#### Métodos de Sesiones

**`ObtenerSesionesPorPeliculaAsync(int peliculaId, DateTime? fecha = null)`**
```csharp
public async Task<List<Sesion>> ObtenerSesionesPorPeliculaAsync(int peliculaId, DateTime? fecha = null)
```
- **Descripción**: Obtiene sesiones de una película específica
- **Parámetros**:
  - `peliculaId`: ID de la película
  - `fecha`: Fecha opcional para filtrar (si es null, muestra todas las futuras)
- **Retorno**: Lista de sesiones con información de película y sala
- **Query**: JOIN entre Sesiones, Peliculas y Salas
- **Filtros**: Solo sesiones activas y futuras
- **Uso**: Ventana de selección de sesión

---

#### Métodos de Butacas

**`ObtenerButacasPorSalaAsync(int salaId)`**
```csharp
public async Task<List<Butaca>> ObtenerButacasPorSalaAsync(int salaId)
```
- **Descripción**: Obtiene todas las butacas de una sala
- **Parámetros**: `salaId` - ID de la sala
- **Retorno**: Lista de butacas ordenadas por fila y columna
- **Query**: `SELECT * FROM Butacas WHERE SalaId = @SalaId AND Activa = 1`
- **Uso**: Ventana de selección de butacas

**`ObtenerButacasReservadasAsync(int sesionId)`**
```csharp
public async Task<List<int>> ObtenerButacasReservadasAsync(int sesionId)
```
- **Descripción**: Obtiene IDs de butacas ya reservadas para una sesión
- **Parámetros**: `sesionId` - ID de la sesión
- **Retorno**: Lista de IDs de butacas ocupadas
- **Query**: JOIN entre ReservasButacas y Reservas
- **Filtro**: Solo reservas en estado 'Pendiente' o 'Confirmada'
- **Uso**: Ventana de selección de butacas (para marcar ocupadas)

---

#### Métodos de Usuarios

**`ValidarUsuarioAsync(string email, string password)`**
```csharp
public async Task<Usuario?> ValidarUsuarioAsync(string email, string password)
```
- **Descripción**: Valida credenciales de un usuario
- **Parámetros**: 
  - `email`: Email del usuario
  - `password`: Contraseña
- **Retorno**: Objeto Usuario si es válido, null si no
- **Query**: `SELECT * FROM Usuarios WHERE Email = @Email AND Password = @Password AND Activo = 1`
- **Nota**:  Password en texto plano (mejorar con hash en producción)
- **Uso**: Ventana de login

**`ExisteUsuarioAsync(string email)`**
```csharp
public async Task<bool> ExisteUsuarioAsync(string email)
```
- **Descripción**: Verifica si un email ya está registrado
- **Parámetros**: `email` - Email a verificar
- **Retorno**: true si existe, false si no
- **Query**: `SELECT COUNT(*) FROM Usuarios WHERE Email = @Email`
- **Uso**: Ventana de registro (validación)

**`RegistrarUsuarioAsync(Usuario usuario)`**
```csharp
public async Task<bool> RegistrarUsuarioAsync(Usuario usuario)
```
- **Descripción**: Registra un nuevo usuario en el sistema
- **Parámetros**: `usuario` - Objeto Usuario con los datos
- **Retorno**: true si se registró correctamente, false si no
- **Query**: INSERT INTO Usuarios
- **Campos**: Nombre, Apellidos, Email, Password, Telefono, FechaRegistro, Activo
- **Uso**: Ventana de registro

**`ActualizarPasswordAsync(int usuarioId, string nuevaPassword)`**
```csharp
public async Task<bool> ActualizarPasswordAsync(int usuarioId, string nuevaPassword)
```
- **Descripción**: Actualiza la contraseña de un usuario
- **Parámetros**:
  - `usuarioId`: ID del usuario
  - `nuevaPassword`: Nueva contraseña
- **Retorno**: true si se actualizó, false si no
- **Query**: `UPDATE Usuarios SET Password = @Password WHERE Id = @Id`
- **Uso**: Ventana de perfil (cambio de contraseña)

---

#### Métodos de Reservas

**`ObtenerReservasPorUsuarioAsync(int usuarioId)`**
```csharp
public async Task<List<Reserva>> ObtenerReservasPorUsuarioAsync(int usuarioId)
```
- **Descripción**: Obtiene todas las reservas activas de un usuario
- **Parámetros**: `usuarioId` - ID del usuario
- **Retorno**: Lista de reservas con información completa
- **Query**: JOIN complejo entre Reservas, Sesiones, Peliculas y Salas
- **Filtro**: Solo reservas en estado 'Pendiente' o 'Confirmada'
- **Carga Adicional**: Llama a `ObtenerButacasDeReservaAsync()` para cada reserva
- **Manejo de Errores**: Try-catch para continuar aunque falle una reserva
- **Uso**: Ventana de perfil (historial de reservas)

**`ObtenerButacasDeReservaAsync(int reservaId)`** (Privado)
```csharp
private async Task<List<ReservaButaca>> ObtenerButacasDeReservaAsync(int reservaId)
```
- **Descripción**: Obtiene las butacas de una reserva específica
- **Parámetros**: `reservaId` - ID de la reserva
- **Retorno**: Lista de ReservaButaca con información de butaca
- **Query**: JOIN entre ReservasButacas y Butacas
- **Uso**: Método auxiliar de `ObtenerReservasPorUsuarioAsync()`

**`CrearReservaAsync(Reserva reserva, List<int> butacaIds)`**
```csharp
public async Task<int> CrearReservaAsync(Reserva reserva, List<int> butacaIds)
```
- **Descripción**: Crea una nueva reserva con transacción
- **Parámetros**:
  - `reserva`: Objeto Reserva con los datos
  - `butacaIds`: Lista de IDs de butacas seleccionadas
- **Retorno**: ID de la reserva creada
- **Transacción**: Utiliza transacción de MySQL para garantizar consistencia
- **Generación de Código**: Crea código único formato `RES{fecha}{random}`

**Proceso de Transacción:**
1. Inicia transacción MySQL
2. Genera código de reserva único
3. Inserta registro en tabla Reservas
4. Para cada butaca seleccionada:
   - Inserta en tabla ReservasButacas
5. Commit de la transacción
6. En caso de error: Rollback automático

---

### 2. **ServicioSesion.cs**
Servicio Singleton para gestionar la sesión del usuario autenticado.

#### Patrón Singleton
```csharp
public class ServicioSesion
{
    private static ServicioSesion? _instance;
    private static readonly object _lock = new object();
    
    public static ServicioSesion Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ServicioSesion();
                    }
                }
            }
            return _instance;
        }
    }
    
    private ServicioSesion() { }
}
```

#### Propiedades
```csharp
public Usuario? UsuarioActual { get; private set; }
public bool EstaAutenticado => UsuarioActual != null;
```

#### Eventos
```csharp
public event EventHandler? SesionIniciada;
public event EventHandler? SesionCerrada;
```

#### Métodos
**`IniciarSesion(Usuario usuario)`**
```csharp
public void IniciarSesion(Usuario usuario)
```
- **Descripción**: Establece el usuario actual y dispara evento
- **Parámetros**: `usuario` - Usuario autenticado
- **Efecto**: Actualiza `UsuarioActual` y dispara `SesionIniciada`

**`CerrarSesion()`**
```csharp
public void CerrarSesion()
```
- **Descripción**: Limpia la sesión actual
- **Efecto**: `UsuarioActual = null` y dispara `SesionCerrada`

**Ventajas del Singleton:**
- ✅ Una única instancia de sesión en toda la aplicación
- ✅ Acceso global desde cualquier ventana
- ✅ Estado consistente del usuario autenticado
- ✅ Sistema de eventos para reaccionar a cambios de sesión

---

## Ventanas de la Aplicación

### 1. **CarteleraWindow** (Ventana Principal) 🎬

#### Descripción
Ventana principal de la aplicación que muestra la cartelera de películas disponibles. Es la primera ventana que se carga al iniciar la aplicación y actúa como hub de navegación.

#### Elementos de UI

**Barra Superior:**
- Logo/Título: "CINEMAX"
- Información de usuario (si está autenticado)
- Botones de acción:
  - "Iniciar Sesión" / "Cerrar Sesión"
  - "Mi Perfil" (solo si está autenticado)

**Grid de Películas:**
- Diseño responsive con WrapPanel
- Cards de películas con:
  - Imagen/Poster
  - Título de la película
  - Información básica (género, duración)
  - Botón "Ver Horarios"

#### Métodos Principales

**`CarteleraWindow_Loaded(object sender, RoutedEventArgs e)`**
- Se ejecuta al cargar la ventana
- Suscribe eventos de ServicioSesion
- Llama a `ActualizarEstadoUsuario()`
- Llama a `CargarPeliculas()`

**`ActualizarEstadoUsuario()`**
```csharp
private void ActualizarEstadoUsuario()
```
- Verifica si hay usuario autenticado
- Actualiza texto del botón de cuenta
- Muestra/oculta botón de perfil
- Actualiza mensaje de bienvenida

**`async CargarPeliculas()`**
```csharp
private async void CargarPeliculas()
```
- Obtiene películas activas desde BD
- Crea cards dinámicamente
- Renderiza grid de películas
- Maneja errores con MessageBox

**`BtnVerHorarios_Click(object sender, RoutedEventArgs e)`**
- Obtiene película seleccionada del Tag del botón
- Abre SeleccionSesionWindow
- Pasa película como parámetro

**`BtnCuentaAccion_Click(object sender, RoutedEventArgs e)`**
- Si está autenticado: Cierra sesión
- Si no está autenticado: Abre LoginWindow
- Actualiza UI después de la acción

**`BtnPerfilUsuario_Click(object sender, RoutedEventArgs e)`**
- Verifica autenticación
- Abre PerfilUsuarioWindow

#### Características
- ✅ Carga dinámica de películas desde BD
- ✅ Gestión de estado de sesión
- ✅ Navegación a otras ventanas
- ✅ Actualización automática al cambiar sesión
- ✅ Diseño responsive

---

### 2. **LoginWindow** (Inicio de sesión) 🔐

#### Descripción
Ventana modal para autenticación de usuarios. Permite iniciar sesión con email y contraseña, acceder como invitado o navegar al registro.

#### Elementos de UI

**Formulario de Login:**
- Campo de texto: Email
- Campo de contraseña: Password (con PasswordBox)
- Botón: "Iniciar Sesión"
- Link: "¿No tienes cuenta? Regístrate"
- Botón: "Continuar como Invitado"

**Diseño:**
- Ventana centrada
- Fondo con gradiente o imagen
- Logo/Título de la aplicación
- Mensajes de error visibles

#### Métodos Principales

**`async BtnLogin_Click(object sender, RoutedEventArgs e)`**
```csharp
private async void BtnLogin_Click(object sender, RoutedEventArgs e)
```
**Proceso:**
1. Valida que campos no estén vacíos
2. Llama a `ValidarUsuarioAsync()` del servicio
3. Si es válido:
   - Inicia sesión con `ServicioSesion`
   - Cierra ventana con `DialogResult = true`
4. Si no es válido:
   - Muestra error con `MostrarError()`

**`BtnRegistro_Click(object sender, RoutedEventArgs e)`**
- Abre RegistroWindow
- Si el registro es exitoso, cierra LoginWindow

**`BtnInvitado_Click(object sender, RoutedEventArgs e)`**
- Cierra la ventana sin autenticar
- Permite navegar como invitado

**`TxtPassword_KeyDown(object sender, KeyEventArgs e)`**
- Detecta tecla Enter
- Ejecuta login automáticamente

**`MostrarError(string mensaje)`**
```csharp
private void MostrarError(string mensaje)
```
- Muestra mensaje de error en TextBlock
- Opcionalmente muestra MessageBox
- Hace visible el panel de error

#### Validaciones
- ✅ Campos no vacíos
- ✅ Email válido (verificado en BD)
- ✅ Password correcto
- ✅ Usuario activo

#### Flujo de Usuario
1. Usuario ingresa credenciales
2. Click en "Iniciar Sesión"
3. Validación en BD
4. Si es correcto → Cierra ventana y vuelve a cartelera
5. Si es incorrecto → Muestra mensaje de error

---

### 3. **RegistroWindow** (Registro de Usuarios) 📝

#### Descripción
Ventana modal para registro de nuevos usuarios. Incluye validación completa de formulario y verificación de email único.

#### Elementos de UI

**Formulario de Registro:**
- Campo: Nombre (requerido)
- Campo: Apellidos (requerido)
- Campo: Email (requerido, único)
- Campo: Teléfono (opcional)
- Campo: Contraseña (requerido, min 6 caracteres)
- Campo: Confirmar Contraseña (requerido)
- Botones:
  - "Registrarse"
  - "Cancelar"

**Panel de Mensajes:**
- Área para mostrar errores de validación
- Mensaje de éxito

#### Métodos Principales

**`async BtnRegistrar_Click(object sender, RoutedEventArgs e)`**
```csharp
private async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
```
**Proceso:**
1. Llama a `ValidarCampos()`
2. Verifica que el email no exista con `ExisteUsuarioAsync()`
3. Crea objeto Usuario
4. Llama a `RegistrarUsuarioAsync()`
5. Muestra mensaje de éxito
6. Cierra ventana con `DialogResult = true`

**`ValidarCampos()`**
```csharp
private bool ValidarCampos()
```
**Validaciones:**
- ✅ Nombre: mínimo 2 caracteres
- ✅ Apellidos: mínimo 2 caracteres
- ✅ Email: formato válido (regex)
- ✅ Teléfono: opcional, si se ingresa mínimo 9 dígitos
- ✅ Contraseña: mínimo 6 caracteres
- ✅ Confirmar contraseña: debe coincidir

**`EsEmailValido(string email)`**
```csharp
private bool EsEmailValido(string email)
```
- Valida formato de email con expresión regular
- Patrón: `^[^@\s]+@[^@\s]+\.[^@\s]+$`

**`MostrarError(string mensaje)`**
- Muestra mensaje de error en TextBlock
- Hace visible el panel de errores
- Aplica estilo de error

**`MostrarExito(string mensaje)`**
- Muestra mensaje de éxito
- Aplica estilo de éxito
- Se muestra brevemente antes de cerrar

**`BtnCancelar_Click(object sender, RoutedEventArgs e)`**
- Cierra ventana sin registrar
- `DialogResult = false`

#### Validaciones en Detalle

| Campo | Validación | Mensaje de Error |
|-------|-----------|------------------|
| Nombre | Min 2 caracteres | "El nombre debe tener al menos 2 caracteres" |
| Apellidos | Min 2 caracteres | "Los apellidos deben tener al menos 2 caracteres" |
| Email | Formato válido | "El formato del email no es válido" |
| Email | Único en BD | "Este email ya está registrado" |
| Teléfono | Min 9 dígitos (opcional) | "El teléfono debe tener al menos 9 dígitos" |
| Password | Min 6 caracteres | "La contraseña debe tener al menos 6 caracteres" |
| Confirmar | Coincide con password | "Las contraseñas no coinciden" |

#### Flujo de Usuario
1. Usuario completa formulario
2. Click en "Registrarse"
3. Validación de campos
4. Verificación de email único
5. Registro en BD
6. Mensaje de éxito
7. Cierra ventana → Vuelve a Login

---

### 4. **SeleccionSesionWindow** (Selección de Horarios) 📅

#### Descripción
Ventana modal que muestra las sesiones disponibles de una película específica. Incluye calendario para selección de fecha y lista de horarios.

#### Elementos de UI

**Información de Película:**
- Imagen/Poster
- Título
- Director
- Duración
- Género
- Calificación

**Calendario:**
- Control Calendar de WPF
- Selección de fecha única
- Fechas futuras habilitadas

**Lista de Sesiones:**
- ItemsControl con sesiones del día seleccionado
- Cada sesión muestra:
  - Hora de inicio
  - Sala
  - Precio
  - Butacas disponibles
  - Botón "Seleccionar"

**Botones:**
- "Cerrar"

#### Propiedades Privadas
```csharp
private Pelicula peliculaActual;
private ServicioBaseDeDatos servicioBD;
```

#### Constructor
```csharp
public SeleccionSesionWindow(Pelicula pelicula)
```
- Recibe película como parámetro
- Inicializa servicio de BD
- Carga información de la película

#### Métodos Principales

**`CargarInfoPelicula()`**
```csharp
private void CargarInfoPelicula()
```
- Muestra información de la película en UI
- Carga imagen desde URL
- Formatea duración (ej: "120 min")
- Muestra calificación

**`Calendario_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)`**
```csharp
private void Calendario_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
```
- Se dispara al cambiar fecha en calendario
- Obtiene fecha seleccionada
- Llama a `CargarSesiones(fechaSeleccionada)`

**`async CargarSesiones(DateTime fecha)`**
```csharp
private async void CargarSesiones(DateTime fecha)
```
**Proceso:**
1. Obtiene sesiones desde BD con `ObtenerSesionesPorPeliculaAsync()`
2. Filtra sesiones por fecha
3. Para cada sesión:
   - Calcula butacas disponibles
   - Crea elemento de UI
   - Añade a ItemsControl
4. Si no hay sesiones: muestra mensaje

**`BtnSeleccionarSesion_Click(object sender, RoutedEventArgs e)`**
```csharp
private void BtnSeleccionarSesion_Click(object sender, RoutedEventArgs e)
```
**Proceso:**
1. Obtiene sesión seleccionada del Tag del botón
2. Verifica autenticación:
   - Si NO está autenticado: Muestra LoginWindow
   - Si está autenticado: Continúa
3. Llama a `AbrirSeleccionButacas(sesion)`

**`AbrirSeleccionButacas(Sesion sesion)`**
```csharp
private void AbrirSeleccionButacas(Sesion sesion)
```
- Crea instancia de SeleccionButacasWindow
- Pasa sesión como parámetro
- Muestra ventana modal
- Al cerrar: recarga sesiones (por si cambió disponibilidad)

**`BtnCerrar_Click(object sender, RoutedEventArgs e)`**
- Cierra la ventana

#### Características
- ✅ Visualización completa de información de película
- ✅ Calendario interactivo
- ✅ Carga dinámica de sesiones por fecha
- ✅ Verificación de autenticación
- ✅ Cálculo de disponibilidad en tiempo real

#### Flujo de Usuario
1. Usuario ve información de película
2. Selecciona fecha en calendario
3. Ve lista de sesiones disponibles
4. Click en "Seleccionar" de una sesión
5. Si no está autenticado → Muestra Login
6. Si está autenticado → Abre selección de butacas

---

### 5. **SeleccionButacasWindow** (Selección de Asientos) 💺

#### Descripción
Ventana modal avanzada que muestra la sala de cine con visualización de butacas en perspectiva 3D. Permite seleccionar asientos y confirmar reserva.

#### Elementos de UI

**Cabecera:**
- Información de sesión:
  - Película
  - Fecha y hora
  - Sala
  - Precio por butaca

**Visualización de Sala:**
- Grid con efecto de perspectiva
- Pantalla de cine en la parte superior
- Butacas organizadas por filas y columnas
- Código de colores:
  - 🟢 Verde: Disponible
  - 🔴 Rojo: Ocupada
  - 🔵 Azul: Seleccionada
  - 🟡 Amarillo: VIP disponible
  - 🟣 Morado: Discapacitado disponible

**Leyenda:**
- Explicación de estados de butacas
- Iconos con colores

**Panel de Resumen:**
- Butacas seleccionadas (lista)
- Cantidad de butacas
- Total a pagar
- Botón "Confirmar Reserva"
- Botón "Cancelar"

#### Propiedades Privadas
```csharp
private Sesion sesionActual;
private ServicioBaseDeDatos servicioBD;
private List<Butaca> todasLasButacas;
private List<int> butacasOcupadas;
private List<Butaca> butacasSeleccionadas;
```

#### Constructor
```csharp
public SeleccionButacasWindow(Sesion sesion)
```
- Recibe sesión como parámetro
- Inicializa colecciones
- Llama a `CargarInformacion()`

#### Métodos Principales

**`async CargarInformacion()`**
```csharp
private async void CargarInformacion()
```
**Proceso:**
1. Muestra información de sesión en UI
2. Llama a `CargarButacas()`

**`async CargarButacas()`**
```csharp
private async void CargarButacas()
```
**Proceso:**
1. Obtiene todas las butacas de la sala
2. Obtiene butacas ya reservadas para esta sesión
3. Llama a `CrearVisualizacionButacas()`

**`CrearVisualizacionButacas()`**
```csharp
private void CrearVisualizacionButacas()
```
**Algoritmo de Perspectiva:**
```
Para cada fila (de atrás hacia adelante):
  - Calcular factor de escala: 1.0 → 0.6
  - Calcular tamaño de butaca según fila
  - Calcular espaciado lateral (efecto cono)
  - Crear botones de butaca
  - Aplicar estado (disponible/ocupada)
```

**Fórmulas de Perspectiva:**
```csharp
scaleFactor = 1.0 - (fila - 1) * 0.05;  // Reduce 5% por fila
buttonSize = baseSize * scaleFactor;
lateralSpacing = (maxWidth - (columnas * buttonSize)) / 2;
```

**`CrearBotonButaca(Butaca butaca, double size, int fila, int columna)`**
```csharp
private Button CrearBotonButaca(Butaca butaca, double size, int fila, int columna)
```
- Crea Button para una butaca
- Aplica estilo según tipo y estado
- Configura evento Click
- Establece propiedades:
  - Width/Height
  - Content (identificador, ej: "A1", "B5")
  - Tag (objeto Butaca)
  - IsEnabled (según si está ocupada)

**`BtnButaca_Click(object sender, RoutedEventArgs e)`**
```csharp
private void BtnButaca_Click(object sender, RoutedEventArgs e)
```
**Lógica de Toggle:**
```csharp
Si butaca está en lista de seleccionadas:
  - Remover de lista
  - Cambiar estilo a "disponible"
Sino:
  - Añadir a lista
  - Cambiar estilo a "seleccionada"
Actualizar resumen
```

**`ActualizarResumen()`**
```csharp
private void ActualizarResumen()
```
- Actualiza TextBlock con lista de butacas seleccionadas
- Calcula total: `cantidad × precio`
- Muestra información en panel de resumen
- Habilita/deshabilita botón de confirmar

**`async BtnConfirmarReserva_Click(object sender, RoutedEventArgs e)`**
```csharp
private async void BtnConfirmarReserva_Click(object sender, RoutedEventArgs e)
```
**Proceso:**
1. Verifica que hay butacas seleccionadas
2. Verifica autenticación
3. Muestra confirmación con MessageBox
4. Si acepta: llama a `ProcesarReserva()`

**`async ProcesarReserva()`**
```csharp
private async Task ProcesarReserva()
```
**Proceso de Reserva:**
1. Crea objeto Reserva
2. Obtiene IDs de butacas seleccionadas
3. Llama a `CrearReservaAsync()` (con transacción)
4. Si es exitoso:
   - Muestra código de reserva
   - Mensaje de éxito
   - Cierra ventana
5. Si falla:
   - Muestra error
   - Mantiene ventana abierta

#### Características Visuales

**Efecto de Perspectiva:**
- Filas traseras más grandes
- Filas delanteras más pequeñas
- Espaciado lateral progresivo (forma de cono)
- Simulación de profundidad

**Estilos de Butacas:**
```csharp
// Normal disponible
Background = Green, Foreground = White

// VIP disponible
Background = Gold, Foreground = Black

// Discapacitado disponible
Background = Purple, Foreground = White

// Ocupada
Background = Red, IsEnabled = false

// Seleccionada
Background = Blue, Foreground = White
```

#### Validaciones
- ✅ No permitir selección de butacas ocupadas
- ✅ Verificar autenticación antes de confirmar
- ✅ Validar que haya al menos una butaca seleccionada
- ✅ Confirmación antes de procesar reserva

#### Flujo de Usuario
1. Usuario ve sala con butacas disponibles/ocupadas
2. Click en butacas para seleccionar (máximo permitido)
3. Ve resumen en tiempo real
4. Click en "Confirmar Reserva"
5. Confirmación final
6. Procesamiento de reserva
7. Mensaje con código de reserva
8. Cierra ventana

---

### 6. **PagoWindow** (Sistema de Pago) 💳

#### Descripción
Ventana modal que simula un sistema de pago completo con múltiples métodos de pago. Permite al usuario seleccionar su método preferido, ingresar los datos correspondientes y validar la información antes de confirmar la reserva.

#### Elementos de UI

**Cabecera:**
- Icono de pago (💳)
- Título: "Realizar Pago"
- Total a pagar (destacado en verde)

**Selección de Método de Pago:**
Tres botones grandes con información:
- 💳 **Tarjeta de Crédito/Débito**
  - Texto: "Visa, Mastercard, American Express"
- 📱 **Bizum**
  - Texto: "Pago instantáneo con tu móvil"
- 🅿️ **PayPal**
  - Texto: "Pago seguro con tu cuenta PayPal"

**Formularios (se muestra uno según método seleccionado):**

1. **Formulario de Tarjeta:**
   - Campo: Número de tarjeta (16 dígitos)
   - Campo: Nombre del titular
   - Campo: Fecha de expiración (MM/AA)
   - Campo: CVV (3-4 dígitos, PasswordBox)

2. **Formulario de Bizum:**
   - Campo: Número de teléfono (9 dígitos)
   - Texto informativo

3. **Formulario de PayPal:**
   - Campo: Correo electrónico
   - Campo: Contraseña (PasswordBox)
   - Texto informativo

**Panel de Error:**
- Área para mostrar mensajes de validación
- Color rojo para errores

**Botones de Acción:**
- "Cancelar" (gris)
- "Pagar" (naranja, deshabilitado hasta seleccionar método)

#### Propiedades Privadas
```csharp
private decimal _montoTotal;
private string _metodoPagoSeleccionado;
public bool PagoExitoso { get; private set; }
```

#### Constructor
```csharp
public PagoWindow(decimal montoTotal)
```
- Recibe monto total como parámetro
- Inicializa propiedades
- Muestra total en UI

#### Métodos de Selección de Método

**`BtnMetodoTarjeta_Click(object sender, RoutedEventArgs e)`**
```csharp
private void BtnMetodoTarjeta_Click(object sender, RoutedEventArgs e)
```
- Establece método = "Tarjeta"
- Muestra formulario de tarjeta
- Resalta botón seleccionado
- Habilita botón "Pagar"

**`BtnMetodoBizum_Click(object sender, RoutedEventArgs e)`**
```csharp
private void BtnMetodoBizum_Click(object sender, RoutedEventArgs e)
```
- Establece método = "Bizum"
- Muestra formulario de Bizum
- Resalta botón seleccionado
- Habilita botón "Pagar"

**`BtnMetodoPaypal_Click(object sender, RoutedEventArgs e)`**
```csharp
private void BtnMetodoPaypal_Click(object sender, RoutedEventArgs e)
```
- Establece método = "PayPal"
- Muestra formulario de PayPal
- Resalta botón seleccionado
- Habilita botón "Pagar"

**`SeleccionarMetodoPago(string metodo)`**
```csharp
private void SeleccionarMetodoPago(string metodo)
```
- Actualiza método seleccionado
- Habilita botón de pagar
- Oculta errores previos

**`MostrarFormulario(Border formulario)`**
```csharp
private void MostrarFormulario(Border formulario)
```
- Oculta todos los formularios
- Muestra solo el formulario seleccionado

**`ResaltarBoton(Button boton)`**
```csharp
private void ResaltarBoton(Button boton)
```
- Restaura estilo de todos los botones
- Aplica borde naranja al botón seleccionado
- Aumenta grosor del borde (3px)

#### Métodos de Validación

**`ValidarFormularioTarjeta()`**
```csharp
private bool ValidarFormularioTarjeta()
```
**Validaciones implementadas:**

1. **Número de Tarjeta:**
   ```csharp
   if (!Regex.IsMatch(txtNumeroTarjeta.Text, @"^\d{16}$"))
       return false;
   ```
   - Debe tener exactamente 16 dígitos
   - Solo números

2. **Nombre del Titular:**
   ```csharp
   if (string.IsNullOrWhiteSpace(txtNombreTarjeta.Text) || 
       txtNombreTarjeta.Text.Length < 3)
       return false;
   ```
   - No vacío
   - Mínimo 3 caracteres

3. **Fecha de Expiración:**
   ```csharp
   if (!Regex.IsMatch(txtFechaExpiracion.Text, @"^(0[1-9]|1[0-2])\/\d{2}$"))
       return false;
   ```
   - Formato MM/AA (ejemplo: 12/25)
   - Mes válido (01-12)
   - **Validación de fecha no vencida:**
     ```csharp
     var partes = txtFechaExpiracion.Text.Split('/');
     int mes = int.Parse(partes[0]);
     int anio = int.Parse(partes[1]) + 2000;
     var fechaExpiracion = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes));
     
     if (fechaExpiracion < DateTime.Now)
         return false; // Tarjeta vencida
     ```

4. **CVV:**
   ```csharp
   if (!Regex.IsMatch(txtCVV.Password, @"^\d{3,4}$"))
       return false;
   ```
   - 3 o 4 dígitos
   - Solo números

**`ValidarFormularioBizum()`**
```csharp
private bool ValidarFormularioBizum()
```
**Validaciones implementadas:**

1. **Número de Teléfono:**
   ```csharp
   if (!Regex.IsMatch(txtTelefonoBizum.Text, @"^[679]\d{8}$"))
       return false;
   ```
   - Exactamente 9 dígitos
   - Debe empezar por 6, 7 o 9 (números españoles)
   - Solo números

**`ValidarFormularioPaypal()`**
```csharp
private bool ValidarFormularioPaypal()
```
**Validaciones implementadas:**

1. **Correo Electrónico:**
   ```csharp
   if (!Regex.IsMatch(txtEmailPaypal.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
       return false;
   ```
   - Formato válido de email
   - Debe contener @ y dominio
   - Ejemplo: usuario@correo.com

2. **Contraseña:**
   ```csharp
   if (txtPasswordPaypal.Password.Length < 6)
       return false;
   ```
   - Mínimo 6 caracteres

#### Procesamiento de Pago

**`async BtnPagar_Click(object sender, RoutedEventArgs e)`**
```csharp
private async void BtnPagar_Click(object sender, RoutedEventArgs e)
```
**Proceso completo:**

1. **Validación según método:**
   ```csharp
   bool esValido = _metodoPagoSeleccionado switch
   {
       "Tarjeta" => ValidarFormularioTarjeta(),
       "Bizum" => ValidarFormularioBizum(),
       "PayPal" => ValidarFormularioPaypal(),
       _ => false
   };
   ```

2. **Si la validación falla:**
   - Muestra mensaje de error específico
   - Mantiene ventana abierta
   - Permite corregir datos

3. **Si la validación es exitosa:**
   - Deshabilita botón "Pagar"
   - Cambia texto a "Procesando..."
   - Simula procesamiento (2 segundos)
   ```csharp
   await Task.Delay(2000);
   ```

4. **Pago exitoso:**
   - Establece `PagoExitoso = true`
   - Muestra MessageBox con confirmación:
     ```
     ¡Pago procesado correctamente!
     
     Método de pago: Tarjeta
     Monto: 30.00€
     
     Recibirás un correo de confirmación...
     ```
   - Cierra ventana con `DialogResult = true`

**`BtnCancelar_Click(object sender, RoutedEventArgs e)`**
```csharp
private void BtnCancelar_Click(object sender, RoutedEventArgs e)
```
- Muestra confirmación
- Si confirma: Cierra con `DialogResult = false`
- La reserva NO se procesa

#### Métodos Auxiliares

**`MostrarError(string mensaje)`**
```csharp
private void MostrarError(string mensaje)
```
- Actualiza texto del error
- Hace visible el panel de error
- Aplica estilo rojo

**`OcultarError()`**
```csharp
private void OcultarError()
```
- Oculta el panel de error
- Se llama al seleccionar nuevo método

#### Tabla de Validaciones por Método

| Método | Campo | Validación | Regex/Lógica | Ejemplo Válido |
|--------|-------|-----------|--------------|----------------|
| **Tarjeta** | Número | 16 dígitos | `^\d{16}$` | `4532015112830366` |
| | Titular | Min 3 chars | Length >= 3 | `JUAN PEREZ` |
| | Expiración | MM/AA, futuro | `^(0[1-9]\|1[0-2])\/\d{2}$` + Date | `12/25` |
| | CVV | 3-4 dígitos | `^\d{3,4}$` | `123` |
| **Bizum** | Teléfono | 9 dígitos, 6/7/9 | `^[679]\d{8}$` | `666555444` |
| **PayPal** | Email | Formato válido | `^[^@\s]+@[^@\s]+\.[^@\s]+$` | `user@gmail.com` |
| | Password | Min 6 chars | Length >= 6 | `pass123` |

#### Mensajes de Error Específicos

**Tarjeta:**
- "Por favor, ingresa el número de tarjeta."
- "El número de tarjeta debe tener exactamente 16 dígitos."
- "Por favor, ingresa el nombre del titular (mínimo 3 caracteres)."
- "Por favor, ingresa la fecha de expiración."
- "La fecha de expiración debe tener el formato MM/AA (ejemplo: 12/25)."
- "La tarjeta está vencida. Por favor, usa una tarjeta válida."
- "Por favor, ingresa el código CVV."
- "El CVV debe tener 3 o 4 dígitos."

**Bizum:**
- "Por favor, ingresa tu número de teléfono."
- "El número de teléfono debe tener 9 dígitos y comenzar por 6, 7 o 9."

**PayPal:**
- "Por favor, ingresa tu correo electrónico de PayPal."
- "El correo electrónico no es válido. Debe tener el formato: ejemplo@correo.com"
- "Por favor, ingresa tu contraseña de PayPal."
- "La contraseña debe tener al menos 6 caracteres."

#### Características de Seguridad

✅ **Validación en Cliente:**
- Validación inmediata antes de enviar
- Mensajes de error claros
- Prevención de datos inválidos

✅ **Campos Sensibles:**
- CVV usa PasswordBox (oculta caracteres)
- Password de PayPal usa PasswordBox

✅ **Simulación Realista:**
- Tiempo de procesamiento (2 segundos)
- Mensajes de confirmación
- Estado de botón durante procesamiento

⚠️ **Nota de Seguridad:**
Esta es una **simulación educativa**. En producción:
- NO almacenar datos de tarjeta
- Usar pasarela de pago real (Stripe, PayPal API)
- Implementar HTTPS
- Cumplir con PCI DSS
- Tokenización de datos sensibles

---

### 7. **PerfilUsuarioWindow** (Perfil y Reservas) 👤

#### Descripción
Ventana modal que muestra el perfil del usuario autenticado, permite cambiar contraseña y ver historial de reservas.

#### Elementos de UI

**Menú Lateral:**
- Botón: "Información Personal"
- Botón: "Mis Reservas"

**Panel de Información Personal:**
- Datos del usuario:
  - Nombre completo
  - Email
  - Teléfono
  - Fecha de registro
- Sección "Cambiar Contraseña":
  - Campo: Contraseña actual
  - Campo: Nueva contraseña
  - Campo: Confirmar nueva contraseña
  - Botón: "Cambiar Contraseña"

**Panel de Mis Reservas:**
- Lista de reservas activas
- Cada reserva muestra:
  - Código de reserva
  - Película
  - Fecha y hora de sesión
  - Sala
  - Butacas reservadas
  - Total pagado
  - Estado (Pendiente/Confirmada)

**Botones:**
- "Volver"

#### Propiedades Privadas
```csharp
private Usuario usuarioActual;
private ServicioBaseDeDatos servicioBD;
```

#### Constructor
```csharp
public PerfilUsuarioWindow()
```
- Obtiene usuario actual de ServicioSesion
- Inicializa servicio de BD
- Llama a `CargarInformacionUsuario()`
- Muestra panel de información por defecto

#### Métodos Principales

**`CargarInformacionUsuario()`**
```csharp
private void CargarInformacionUsuario()
```
- Muestra nombre completo
- Muestra email
- Muestra teléfono (o "No especificado")
- Muestra fecha de registro formateada

**`BtnMenuInformacion_Click(object sender, RoutedEventArgs e)`**
- Oculta panel de reservas
- Muestra panel de información
- Actualiza estilo de botones de menú

**`BtnMenuReservas_Click(object sender, RoutedEventArgs e)`**
- Oculta panel de información
- Muestra panel de reservas
- Llama a `CargarReservas()`
- Actualiza estilo de botones de menú

**`async CargarReservas()`**
```csharp
private async void CargarReservas()
```
**Proceso:**
1. Obtiene reservas del usuario desde BD
2. Si no hay reservas: muestra mensaje
3. Para cada reserva:
   - Crea ReservaViewModel
   - Formatea butacas (ej: "A1, A2, B3")
   - Añade a ItemsControl
4. Maneja errores con try-catch

**`async BtnCambiarPassword_Click(object sender, RoutedEventArgs e)`**
```csharp
private async void BtnCambiarPassword_Click(object sender, RoutedEventArgs e)
```
**Validaciones:**
1. Verifica que campos no estén vacíos
2. Valida contraseña actual
3. Verifica que nueva contraseña tenga mínimo 6 caracteres
4. Verifica que contraseñas nuevas coincidan
5. Llama a `ActualizarPasswordAsync()`
6. Muestra mensaje de éxito/error
7. Limpia campos

**`MostrarMensajePassword(string mensaje, bool esError)`**
```csharp
private void MostrarMensajePassword(string mensaje, bool esError)
```
- Muestra mensaje en TextBlock
- Aplica estilo según tipo (error/éxito)
- Hace visible el panel de mensaje

**`BtnVolver_Click(object sender, RoutedEventArgs e)`**
- Cierra la ventana

#### DataTemplate de Reservas

**Información Mostrada:**
```
┌──────────────────────────────────────────┐
│ Reserva: RES20240115ABCD                 │
│ Película: Oppenheimer                    │
│ Sesión: 15/01/2024 20:30 - Sala 1        │
│ Butacas: A1, A2, A3                      │
│ Total: €30.00                            │
│ Estado: Confirmada                       │
└──────────────────────────────────────────┘

```

#### Validaciones de Cambio de Contraseña

| Validación | Mensaje de Error |
|-----------|------------------|
| Campos vacíos | "Por favor, complete todos los campos" |
| Contraseña actual incorrecta | "La contraseña actual no es correcta" |
| Nueva contraseña corta | "La nueva contraseña debe tener al menos 6 caracteres" |
| Contraseñas no coinciden | "Las contraseñas nuevas no coinciden" |
| Error en BD | "Error al cambiar la contraseña" |

#### Características
- ✅ Visualización de datos de perfil
- ✅ Cambio seguro de contraseña
- ✅ Historial completo de reservas
- ✅ Formato claro de información
- ✅ Manejo de errores

---

## 2. Flujo de Reserva de Película

```
┌─────────────────────────────────────────────────────────────┐
│                 FLUJO DE RESERVA DE PELÍCULA                │
└─────────────────────────────────────────────────────────────┘

1. Usuario ve CarteleraWindow
   │
   └─► Click en "Ver Horarios" de una película
       │
       └─► Abre SeleccionSesionWindow
           │
           ├─► Muestra información de película
           ├─► Usuario selecciona fecha en calendario
           ├─► Se cargan sesiones del día
           │
           └─► Click en "Seleccionar" de una sesión
               │
               │
               ├─► Verifica autenticación
               │   │
               │   ├─► Si NO está autenticado:
               │   │   └─► Abre LoginWindow
               │   │       └─► Después de login exitoso:
               │   │           └─► Continúa con reserva
               │   │
               │   └─► Si está autenticado:
               │       └─► Abre SeleccionButacasWindow
               │           │
               │           ├─► Muestra sala con butacas
               │           ├─► Usuario selecciona butacas
               │           ├─► Ve resumen en tiempo real
               │           │
               │           └─► Click en "Confirmar Reserva"
               │               │
               │               └─► Abre PagoWindow 💳
               │                   │
               │                   ├─► Muestra total a pagar
               │                   ├─► Usuario selecciona método de pago:
               │                   │   ├─► Tarjeta
               │                   │   ├─► Bizum
               │                   │   └─► PayPal
               │                   │
               │                   ├─► Usuario completa formulario
               │                   │
               │                   ├─► Click en "Pagar"
               │                   │   │
               │                   │   ├─► Valida datos según método
               │                   │   │
               │                   │   ├─► Si hay errores:
               │                   │   │   └─► Muestra mensaje
               │                   │   │       └─► Usuario corrige
               │                   │   │
               │                   │   └─► Si es válido:
               │                   │       ├─► Simula procesamiento (2 seg)
               │                   │       ├─► Muestra confirmación
               │                   │       └─► Cierra PagoWindow (éxito)
               │                   │
               │                   └─► Vuelve a SeleccionButacasWindow
               │                       │
               │                       ├─► Si pago exitoso:
               │                       │   ├─► Procesa reserva (transacción BD)
               │                       │   ├─► Genera código de reserva
               │                       │   ├─► Muestra mensaje de éxito
               │                       │   └─► Cierra SeleccionButacasWindow
               │                       │       └─► Vuelve a SeleccionSesionWindow
               │                       │
               │                       └─► Si pago cancelado:
               │                           └─► Mantiene butacas seleccionadas
               │                               └─► Usuario puede reintentar
               │
               └─► Si hay error:
                   └─► Muestra mensaje de error
                       └─► Mantiene ventana abierta
