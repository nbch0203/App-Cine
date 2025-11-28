# ?? Guía de Contribución

¡Gracias por tu interés en contribuir al proyecto de Sistema de Reserva de Cine!

## ?? Tabla de Contenidos
- [Código de Conducta](#código-de-conducta)
- [¿Cómo puedo contribuir?](#cómo-puedo-contribuir)
- [Configuración del Entorno de Desarrollo](#configuración-del-entorno-de-desarrollo)
- [Proceso de Contribución](#proceso-de-contribución)
- [Guía de Estilo](#guía-de-estilo)
- [Estructura del Proyecto](#estructura-del-proyecto)

---

## Código de Conducta

Este proyecto está destinado al aprendizaje y la colaboración respetuosa. Por favor:
- Sé respetuoso con otros contribuyentes
- Acepta críticas constructivas
- Enfócate en lo que es mejor para la comunidad
- Muestra empatía hacia otros miembros

---

## ¿Cómo puedo contribuir?

### ?? Reportar Bugs
1. Verifica que el bug no haya sido reportado antes
2. Abre un nuevo issue con:
   - Título descriptivo
   - Pasos para reproducir el error
   - Comportamiento esperado vs. real
   - Capturas de pantalla (si aplica)
   - Versión de .NET, Visual Studio y MySQL

### ?? Sugerir Mejoras
1. Abre un issue describiendo tu sugerencia
2. Explica por qué sería útil para el proyecto
3. Incluye ejemplos de uso si es posible

### ?? Contribuir con Código
1. Haz fork del repositorio
2. Crea una rama para tu feature (`git checkout -b feature/nueva-funcionalidad`)
3. Realiza tus cambios
4. Haz commit de tus cambios (`git commit -m 'Añadir nueva funcionalidad'`)
5. Haz push a la rama (`git push origin feature/nueva-funcionalidad`)
6. Abre un Pull Request

---

## Configuración del Entorno de Desarrollo

### Requisitos Previos
- Visual Studio 2022 o superior
- .NET 8.0 SDK o superior
- MySQL Server 8.0+
- Git

### Instalación

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/nbch0203/App-de-cine.git
   cd App-de-cine
   ```

2. **Configurar la base de datos:**
   - Ejecutar `Cine_app/Database/cinema_database_mysql.sql` en MySQL
   - Crear base de datos `cinema_db`

3. **Configurar archivo .env:**
   ```bash
   cp .env.example .env
   # Editar .env con tus credenciales de MySQL
   ```

4. **Restaurar paquetes NuGet:**
   ```bash
   dotnet restore
   ```

5. **Compilar el proyecto:**
   ```bash
   dotnet build
   ```

---

## Proceso de Contribución

### 1. Antes de Empezar
- [ ] Revisa los issues abiertos
- [ ] Comenta en el issue en el que quieres trabajar
- [ ] Espera confirmación antes de empezar

### 2. Durante el Desarrollo
- [ ] Sigue las convenciones de código existentes
- [ ] Comenta tu código donde sea necesario
- [ ] Escribe código limpio y legible
- [ ] Prueba tus cambios exhaustivamente

### 3. Antes de Hacer Push
- [ ] Compila el proyecto sin errores
- [ ] Prueba todas las funcionalidades afectadas
- [ ] Actualiza la documentación si es necesario
- [ ] Verifica que no hayas incluido archivos sensibles

### 4. Pull Request
- [ ] Título descriptivo y claro
- [ ] Descripción detallada de los cambios
- [ ] Referencias a issues relacionados
- [ ] Capturas de pantalla (si aplica)

---

## Guía de Estilo

### C#
```csharp
// Nombres de clases: PascalCase
public class ServicioBaseDeDatos { }

// Nombres de métodos: PascalCase
public async Task<List<Pelicula>> ObtenerPeliculasActivasAsync() { }

// Variables privadas: camelCase con _
private readonly ServicioBaseDeDatos _servicioBD;

// Variables locales y parámetros: camelCase
var peliculas = await servicioBD.ObtenerPeliculasActivasAsync();

// Constantes: UPPER_CASE
private const string CONNECTION_STRING = "...";

// Comentarios XML para métodos públicos
/// <summary>
/// Obtiene todas las películas activas de la base de datos
/// </summary>
/// <returns>Lista de películas activas</returns>
public async Task<List<Pelicula>> ObtenerPeliculasActivasAsync() { }
```

### XAML
```xml
<!-- Nombres de controles: PascalCase con prefijo -->
<Button x:Name="BtnLogin" ... />
<TextBlock x:Name="TxtNombre" ... />
<TextBox x:Name="TxtEmail" ... />

<!-- Indentación: 4 espacios -->
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
    </Grid.RowDefinitions>
</Grid>

<!-- Comentarios descriptivos -->
<!-- BARRA SUPERIOR -->
<Border Grid.Row="0" ... />
```

### SQL
```sql
-- Nombres de tablas: PascalCase singular
CREATE TABLE Usuario (...);

-- Nombres de columnas: PascalCase
CREATE TABLE Pelicula (
    Id INT PRIMARY KEY,
    Titulo VARCHAR(200),
    FechaEstreno DATE
);

-- Usar comentarios descriptivos
-- Crear índice para búsquedas por email
CREATE INDEX idx_usuario_email ON Usuarios(Email);
```

### Commits
```bash
# Formato: <tipo>: <descripción>
# Ejemplos:
feat: Añadir funcionalidad de búsqueda de películas
fix: Corregir error en cálculo de precio total
docs: Actualizar README con instrucciones de instalación
style: Mejorar espaciado en CarteleraWindow
refactor: Reorganizar métodos de ServicioBaseDeDatos
test: Añadir pruebas para validación de email
chore: Actualizar paquetes NuGet
```

---

## Estructura del Proyecto

```
App-de-cine/
?
??? ?? Cine_app/                    # Proyecto principal
?   ??? ?? Modelos/                 # Clases de datos
?   ?   ??? Usuario.cs
?   ?   ??? Pelicula.cs
?   ?   ??? Sesion.cs
?   ?   ??? Butaca.cs
?   ?
?   ??? ?? Servicios/               # Lógica de negocio
?   ?   ??? ServicioBaseDeDatos.cs
?   ?   ??? ServicioSesion.cs
?   ?
?   ??? ?? Ventanas/                # Interfaces de usuario
?   ?   ??? CarteleraWindow.xaml
?   ?   ??? LoginWindow.xaml
?   ?   ??? RegistroWindow.xaml
?   ?   ??? SeleccionSesionWindow.xaml
?   ?   ??? SeleccionButacasWindow.xaml
?   ?   ??? PerfilUsuarioWindow.xaml
?   ?
?   ??? ?? Database/                # Scripts SQL
?   ?   ??? cinema_database_mysql.sql
?   ?
?   ??? App.xaml
?   ??? Cine_app.csproj
?
??? ?? Guia paso a paso/            # Documentación educativa
?   ??? GUIA_PASO_A_PASO.md
?   ??? GUIA_PASO_A_PASO_PARTE_2.md
?   ??? GUIA_PASO_A_PASO_PARTE_3.md
?   ??? GUIA_PASO_A_PASO_PARTE_4.md
?
??? .gitignore                      # Archivos ignorados por Git
??? .env.example                    # Ejemplo de configuración
??? README.md                       # Documentación principal
??? CONTRIBUTING.md                 # Esta guía
??? Cine_app.sln                    # Solución de Visual Studio
```

---

## Áreas que Necesitan Contribuciones

### ?? Funcionalidades Prioritarias
- [ ] Sistema de calificación de películas por usuarios
- [ ] Filtros de búsqueda en cartelera (género, fecha, etc.)
- [ ] Sistema de cancelación de reservas
- [ ] Notificaciones por email
- [ ] Panel de administración

### ?? Bugs Conocidos
- [ ] (Agregar bugs conocidos aquí)

### ?? Documentación
- [ ] Tutorial en video
- [ ] Traducción a inglés
- [ ] Diagramas de arquitectura
- [ ] Casos de prueba documentados

### ?? Mejoras de UI/UX
- [ ] Animaciones de transición
- [ ] Tema oscuro/claro
- [ ] Diseño responsive
- [ ] Accesibilidad

---

## Preguntas Frecuentes

**¿Puedo usar este proyecto para mi portafolio?**
Sí, el proyecto es educativo y puedes usarlo libremente.

**¿Necesito pedir permiso para trabajar en un issue?**
Es recomendable comentar en el issue primero para evitar trabajo duplicado.

**¿Cuánto tiempo toma revisar un Pull Request?**
Intentamos revisar PRs en 2-3 días, pero puede variar.

**¿Qué pasa si mi PR no es aceptado?**
Recibirás feedback constructivo sobre qué cambiar para que sea aceptado.

---

## Contacto

- **Repositorio:** [https://github.com/nbch0203/App-de-cine](https://github.com/nbch0203/App-de-cine)
- **Issues:** [https://github.com/nbch0203/App-de-cine/issues](https://github.com/nbch0203/App-de-cine/issues)

---

**¡Gracias por contribuir! ??**

Tu ayuda hace que este proyecto educativo sea mejor para todos.
