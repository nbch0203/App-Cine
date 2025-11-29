# 🎬 GUÍA COMPLETA - SISTEMA DE RESERVA DE CINE

## 📚 Índice de la Guía

Esta guía está dividida en **4 partes** para facilitar su lectura y seguimiento. Cada parte cubre aspectos específicos del desarrollo del proyecto.

---

## 📖 Estructura de la Guía

### 📄 [PARTE 1: Preparación y Fundamentos](Guia%20paso%20a%20paso/GUIA_PASO_A_PASO.md)
**Duración estimada:** 2-3 horas

**Contenido:**
- ✅ Requisitos previos y herramientas necesarias
- ✅ Creación del proyecto en Visual Studio
- ✅ Instalación de paquetes NuGet (MySQL, DotNetEnv)
- ✅ Estructura de carpetas del proyecto
- ✅ Configuración de la base de datos MySQL
- ✅ Creación de todas las tablas
- ✅ Inserción de datos de prueba
- ✅ Configuración del archivo .env
- ✅ **MODELOS**: Creación de todas las clases (Usuario, Pelicula, Sesion, Butaca, Reserva)
- ✅ **SERVICIOS**: ServicioBaseDeDatos completo con todos los métodos
- ✅ **SERVICIOS**: ServicioSesion (patrón Singleton)

**Lo que aprenderás:**
- Configuración de proyectos WPF
- Conexión a MySQL
- Modelado de datos
- Patrón Repository
- Patrón Singleton
- Programación asíncrona

---

### 📄 [PARTE 2: Ventanas Básicas](Guia%20paso%20a%20paso/GUIA_PASO_A_PASO_PARTE_2.md)
**Duración estimada:** 3-4 horas

**Contenido:**
- ✅ Configuración de App.xaml y App.xaml.cs
- ✅ **CarteleraWindow**: Ventana principal con grid de películas
  - Diseño XAML completo
  - Código C# con carga dinámica de películas
  - Gestión de estado de sesión
- ✅ **LoginWindow**: Ventana de inicio de sesión
  - Formulario de login
  - Validación de credenciales
  - Manejo de errores
- ✅ **RegistroWindow**: Ventana de registro de usuarios
  - Formulario completo de registro
  - Validaciones (email, contraseña, etc.)
  - Verificación de email único

**Lo que aprenderás:**
- Diseño de interfaces con XAML
- Creación de formularios
- Validación de datos de usuario
- Eventos y manejo de clicks
- Navegación entre ventanas

---

### 📄 [PARTE 3: Ventanas Avanzadas](Guia%20paso%20a%20paso/GUIA_PASO_A_PASO_PARTE_3.md)
**Duración estimada:** 4-5 horas

**Contenido:**
- ✅ **SeleccionSesionWindow**: Selección de horarios
  - Calendario interactivo
  - Lista dinámica de sesiones
  - Data binding con ItemsControl
  - Verificación de autenticación
- ✅ **SeleccionButacasWindow**: Selección visual de butacas (¡LA MÁS COMPLEJA!)
  - Visualización 3D con efecto de perspectiva
  - Grid dinámico de butacas
  - Estados de butacas (disponible, ocupada, seleccionada)
  - Tipos de butacas (Normal, VIP, Discapacitado)
  - Cálculo en tiempo real del total
  - Transacciones en base de datos

**Lo que aprenderás:**
- Controles avanzados (Calendar, ItemsControl)
- Data binding y DataTemplates
- Creación dinámica de controles
- Efectos visuales (perspectiva 3D)
- Transacciones en BD
- Manejo de estados complejos

---

### 📄 [PARTE 4: Perfil, Configuración y Pruebas](Guia%20paso%20a%20paso/GUIA_PASO_A_PASO_PARTE_4.md)
**Duración estimada:** 2-3 horas

**Contenido:**
- ✅ **PerfilUsuarioWindow**: Gestión de perfil
  - Panel de información personal
  - Cambio de contraseña
  - Historial de reservas
  - DataTemplates para mostrar reservas
- ✅ Configuración final de App.xaml
- ✅ Verificación del .csproj
- ✅ **Pruebas completas** de todas las funcionalidades
- ✅ **Solución de problemas comunes**:
  - Errores de conexión a MySQL
  - Problemas con tablas
  - Imágenes que no cargan
  - Errores de reserva
- ✅ **Mejoras futuras** sugeridas
- ✅ Checklist final
- ✅ Recursos adicionales para seguir aprendiendo

**Lo que aprenderás:**
- Gestión de perfiles de usuario
- Actualización de datos
- Testing de aplicaciones
- Debugging y solución de problemas
- Mejores prácticas

---

## 🎯 Flujo de Aprendizaje Recomendado

```
┌─────────────────────────────────────────────────────────────┐
│                    INICIO DEL PROYECTO                      │
└─────────────────────┬───────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│  PARTE 1: Preparación                                       │
│  ├─ Instalar herramientas                                   │
│  ├─ Crear proyecto                                          │
│  ├─ Configurar MySQL                                        │
│  ├─ Crear modelos                                           │
│  └─ Crear servicios                                         │
└─────────────────────┬───────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│  PARTE 2: Ventanas Básicas                                  │
│  ├─ CarteleraWindow                                         │
│  ├─ LoginWindow                                             │
│  └─ RegistroWindow                                          │
│                                                             │
│  ⚠️ PRUEBA: Verifica que login/registro funcionen          │
└─────────────────────┬───────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│  PARTE 3: Ventanas Avanzadas                                │
│  ├─ SeleccionSesionWindow                                   │
│  └─ SeleccionButacasWindow (la más compleja)                │
│                                                             │
│  ⚠️ PRUEBA: Verifica que las reservas funcionen            │
└─────────────────────┬───────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│  PARTE 4: Final                                             │
│  ├─ PerfilUsuarioWindow                                     │
│  ├─ Configuración final                                     │
│  ├─ Pruebas completas                                       │
│  └─ Solución de problemas                                   │
└─────────────────────┬───────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│               ¡PROYECTO TERMINADO! 🎉                       │
└─────────────────────────────────────────────────────────────┘
```

---

## ⏱️ Tiempo Total Estimado

| Parte | Duración | Dificultad |
|-------|----------|------------|
| Parte 1 | 2-3 horas | ⭐⭐ Fácil |
| Parte 2 | 3-4 horas | ⭐⭐⭐ Media |
| Parte 3 | 4-5 horas | ⭐⭐⭐⭐ Avanzada |
| Parte 4 | 2-3 horas | ⭐⭐⭐ Media |
| **TOTAL** | **11-15 horas** | |

> 💡 **Tip:** No intentes hacerlo todo de una vez. Tómate descansos y ve paso por paso.

---

## 🎓 Nivel del Estudiante

Esta guía está diseñada para estudiantes que:

### ✅ Ya conocen:
- C# básico (variables, clases, métodos)
- Conceptos de programación orientada a objetos
- SQL básico (SELECT, INSERT, UPDATE)
- Visual Studio (lo básico)

### 📚 Van a aprender:
- WPF y XAML
- Conexión a bases de datos
- Programación asíncrona (async/await)
- Patrones de diseño (Singleton, Repository)
- Data binding
- Eventos y navegación
- Transacciones en BD
- Creación de interfaces avanzadas

---

## 📁 Estructura Final del Proyecto

Al terminar, tu proyecto tendrá esta estructura:

```
Cine_app/
│
├── 📂 Modelos/
│   ├── Usuario.cs
│   ├── Pelicula.cs
│   ├── Sesion.cs
│   └── Butaca.cs
│
├── 📂 Servicios/
│   ├── ServicioBaseDeDatos.cs
│   └── ServicioSesion.cs
│
├── 📂 Ventanas/
│   ├── CarteleraWindow.xaml / .cs
│   ├── LoginWindow.xaml / .cs
│   ├── RegistroWindow.xaml / .cs
│   ├── SeleccionSesionWindow.xaml / .cs
│   ├── SeleccionButacasWindow.xaml / .cs
│   ├── **PagoWindow.xaml**           # NUEVA ventana de pago
│   └── PerfilUsuarioWindow.xaml / .cs
│
├── 📂 Database/
│   └── cinema_database_mysql.sql
│
├── 📄 App.xaml / .cs
├── 📄 .env
├── 📄 Cine_app.csproj
└── 📄 .gitignore
```

---

## 🎯 Funcionalidades Completas

Al finalizar la guía, tu aplicación tendrá:

### 🎬 Cartelera
- [x] Visualización de películas activas
- [x] Información detallada de cada película
- [x] Imágenes de posters
- [x] Navegación a horarios

### 👤 Usuarios
- [x] Registro de nuevos usuarios
- [x] Validación de formularios
- [x] Inicio de sesión
- [x] Cierre de sesión
- [x] Gestión de perfil
- [x] Cambio de contraseña

### 📅 Sesiones
- [x] Calendario interactivo
- [x] Lista de horarios por día
- [x] Información de sala y precio
- [x] Verificación de autenticación

### 💺 Reservas
- [x] Visualización 3D de la sala
- [x] Estados de butacas (disponible/ocupada)
- [x] Tipos de butacas (Normal/VIP/Discapacitado)
- [x] Selección múltiple de butacas
- [x] Cálculo automático del total
- [x] **Sistema de pago con múltiples métodos**
- [x] **Validación de datos de pago**
- [x] Generación de código de reserva
- [x] Transacciones seguras

### 💳 Métodos de Pago (NUEVO)
- [x] **Tarjeta de Crédito/Débito**
  - Validación de número (16 dígitos)
  - Validación de fecha de expiración
  - Verificación de CVV
- [x] **Bizum**
  - Validación de teléfono español
- [x] **PayPal**
  - Validación de email
  - Contraseña segura

### 📊 Historial
- [x] Ver reservas activas
- [x] Detalles de cada reserva
- [x] Códigos de reserva

---

## 🛠️ Herramientas Necesarias

Antes de empezar, asegúrate de tener:

| Herramienta | Versión | Obligatorio | Descarga |
|------------|---------|-------------|----------|
| Visual Studio | 2022+ | ✅ Sí | [Link](https://visualstudio.microsoft.com/) |
| .NET SDK | 8.0+ | ✅ Sí | Incluido en VS |
| MySQL Server | 8.0+ | ✅ Sí | [Link](https://dev.mysql.com/downloads/mysql/) |
| MySQL Workbench | 8.0+ | ⭕ Recomendado | [Link](https://dev.mysql.com/downloads/workbench/) |
| Dbeaver | Última | ⭕ La que he usado yo | [Link](https://dbeaver.io/download/) |

---

## 📝 Antes de Empezar

### Checklist Previo:
- [ ] Visual Studio instalado
- [ ] MySQL instalado y ejecutándose
- [ ] Conocimientos básicos de C#
- [ ] Ganas de aprender 😊

### Consejos:
1. **Lee primero, codifica después**: Lee cada sección completa antes de escribir código
2. **Copia el código con cuidado**: Presta atención a los detalles
3. **Prueba frecuentemente**: No esperes al final para probar
4. **Usa el debugger**: Aprende a usar breakpoints (F9)
5. **Lee los errores**: Los mensajes te dicen exactamente qué está mal

---

## 🚀 ¿Listo para Empezar?

Comienza con **[PARTE 1: Preparación y Fundamentos](Guia%20paso%20a%20paso/GUIA_PASO_A_PASO.md)**

---

## 🔧 Instalación y Configuración (Para clonar el repositorio)

Si quieres usar este proyecto clonándolo desde GitHub, sigue estos pasos:

### 1. Clonar el repositorio
```bash
git clone https://github.com/nbch0203/App-de-cine.git
cd App-de-cine
```

### 2. Configurar Visual Studio
1. Abre el archivo de solución `Cine_app.sln` con Visual Studio 2022
2. Visual Studio restaurará automáticamente los paquetes NuGet necesarios

### 3. Configurar MySQL
1. Asegúrate de tener MySQL instalado y ejecutándose
2. Abre MySQL Workbench o tu cliente MySQL favorito
3. Ejecuta el script de base de datos ubicado en:
   ```
   Cine_app/Database/cinema_database_mysql.sql
   ```
4. Esto creará la base de datos `cinema_db` con todas las tablas y datos de prueba

### 4. Configurar archivo .env
1. En la raíz del proyecto, encontrarás un archivo `.env.example`
2. Copia este archivo y renómbralo a `.env`
3. Edita el archivo `.env` con tus credenciales de MySQL:
   ```env
   DATABASE=server=localhost;port=3306;database=cinema_db;user=root;password=TU_CONTRASEÑA
   ```
4. Reemplaza `TU_CONTRASEÑA` con tu contraseña real de MySQL

### 5. Ejecutar la aplicación
1. En Visual Studio, presiona **F5** o click en el botón ▶️ "Iniciar"
2. La aplicación debería compilar y ejecutarse correctamente

### ⚠️ Solución de Problemas al Clonar

**Problema: "No se encuentra el archivo .env"**
- Asegúrate de haber creado el archivo `.env` (sin la extensión .example)
- Verifica que el archivo esté en la raíz del proyecto

**Problema: "Error de conexión a MySQL"**
- Verifica que MySQL esté ejecutándose
- Comprueba que las credenciales en `.env` sean correctas
- Asegúrate de que la base de datos `cinema_db` exista

**Problema: "Faltan paquetes NuGet"**
- Click derecho en la solución → "Restaurar paquetes NuGet"
- O ejecuta en la consola de paquetes:
  ```
  Update-Package -reinstall
  ```

---

## 🆘 Ayuda y Soporte

### Características Principales
- ✅ Visualización de cartelera de películas activas
- ✅ Selección de sesiones por fecha con calendario interactivo
- ✅ Selección visual de butacas con efecto de perspectiva 3D
- ✅ **Sistema de pago simulado con múltiples métodos** (Tarjeta, Bizum, PayPal)
- ✅ Validaciones completas de datos de pago
- ✅ Sistema de autenticación de usuarios
- ✅ Registro de nuevos usuarios con validación
- ✅ Gestión de perfil y cambio de contraseña
- ✅ Historial de reservas personalizado
- ✅ Generación automática de códigos de reserva únicos
- ✅ Manejo inteligente de estados de butacas
- ✅ Soporte para diferentes tipos de butacas (Normal, VIP, Discapacitado)
- ✅ Interfaz moderna con diseño consistente