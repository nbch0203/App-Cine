using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Cine_app.Modelos;
using Cine_app.Servicios;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Cine_app.Ventanas
{
    public partial class SeleccionSesionWindow : Window
    {
        private readonly ServicioBaseDeDatos _dbService;
        private readonly Pelicula _pelicula;
        private HashSet<DateTime> _fechasConSesiones = new HashSet<DateTime>();

        public SeleccionSesionWindow(Pelicula pelicula)
        {
            InitializeComponent();
            _dbService = new ServicioBaseDeDatos();
            _pelicula = pelicula;

            CargarInfoPelicula();
            ConfigurarCalendario();
        }

        private async void ConfigurarCalendario()
        {
            // Configurar el calendario para que se muestre noviembre 2025 inicialmente
            // pero permitir navegación a otros meses
            calendario.DisplayDate = new DateTime(2025, 11, 1);
            
            // Establecer rango de fechas permitidas (por ejemplo, desde hoy hasta fin de año 2025)
            calendario.DisplayDateStart = DateTime.Today;
            calendario.DisplayDateEnd = new DateTime(2025, 12, 31);
            
            // Cargar las fechas que tienen sesiones
            await CargarFechasConSesiones();
            
            // Seleccionar la primera fecha disponible
            if (_fechasConSesiones.Any())
            {
                var primeraFecha = _fechasConSesiones.OrderBy(f => f).First();
                calendario.SelectedDate = primeraFecha;
            }
        }

        private async Task CargarFechasConSesiones()
        {
            try
            {
                // Obtener todas las sesiones de la película sin filtro de fecha específica
                var todasLasSesiones = await _dbService.ObtenerSesionesPorPeliculaAsync(_pelicula.Id, null);
                
                // Extraer todas las fechas únicas con sesiones
                _fechasConSesiones = todasLasSesiones
                    .Select(s => s.FechaHora.Date)
                    .Distinct()
                    .ToHashSet();
                
                // Marcar como deshabilitadas todas las fechas sin sesiones
                // Desde hoy hasta fin de 2025
                calendario.BlackoutDates.Clear();
                
                DateTime fechaInicio = DateTime.Today;
                DateTime fechaFin = new DateTime(2025, 12, 31);
                
                for (DateTime fecha = fechaInicio; fecha <= fechaFin; fecha = fecha.AddDays(1))
                {
                    if (!_fechasConSesiones.Contains(fecha))
                    {
                        calendario.BlackoutDates.Add(new CalendarDateRange(fecha));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar fechas disponibles: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void CargarInfoPelicula()
        {
            txtTitulo.Text = _pelicula.Titulo;

            var info = $"{_pelicula.Genero}";
            if (_pelicula.Duracion.HasValue)
                info += $" • {_pelicula.Duracion} min";
            if (!string.IsNullOrEmpty(_pelicula.Director))
                info += $" • {_pelicula.Director}";

            txtInfo.Text = info;

            if (!string.IsNullOrEmpty(_pelicula.ImagenUrl))
            {
                imgPelicula.Source = new System.Windows.Media.Imaging.BitmapImage(
                    new Uri(_pelicula.ImagenUrl, UriKind.RelativeOrAbsolute));
            }
        }

        private async void Calendario_SelectedDatesChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (calendario.SelectedDate.HasValue)
            {
                await CargarSesiones(calendario.SelectedDate.Value);
            }
        }

        private async Task CargarSesiones(DateTime fecha)
        {
            try
            {
                pnlLoadingSesiones.Visibility = Visibility.Visible;
                scrollSesiones.Visibility = Visibility.Collapsed;
                pnlSinSesiones.Visibility = Visibility.Collapsed;

                var sesiones = await _dbService.ObtenerSesionesPorPeliculaAsync(_pelicula.Id, fecha);

                if (sesiones.Any())
                {
                    itemsSesiones.ItemsSource = sesiones;
                    scrollSesiones.Visibility = Visibility.Visible;
                }
                else
                {
                    pnlSinSesiones.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar sesiones: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                pnlSinSesiones.Visibility = Visibility.Visible;
            }
            finally
            {
                pnlLoadingSesiones.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnSeleccionarSesion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is Sesion sesion)
            {
                // Verificar si el usuario está autenticado antes de continuar
                if (!ServicioSesion.Instance.EstaAutenticado)
                {
                    var result = MessageBox.Show(
                        "Debes iniciar sesión para poder reservar entradas.\n\n¿Deseas iniciar sesión ahora?",
                        "Iniciar Sesión Requerido",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information);

                    if (result == MessageBoxResult.Yes)
                    {
                        var loginWindow = new LoginWindow();
                        bool? loginResult = loginWindow.ShowDialog();

                        // Si después del login el usuario está autenticado, continuar
                        if (ServicioSesion.Instance.EstaAutenticado)
                        {
                            AbrirSeleccionButacas(sesion);
                        }
                        else
                        {
                            // Usuario canceló el login
                            MessageBox.Show("No se puede continuar sin iniciar sesión.", 
                                "Aviso", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Information);
                        }
                    }
                    return;
                }

                // Usuario autenticado, abrir ventana de selección de butacas
                AbrirSeleccionButacas(sesion);
            }
        }

        private void AbrirSeleccionButacas(Sesion sesion)
        {
            // Abrir ventana de selección de butacas
            var seleccionButacasWindow = new SeleccionButacasWindow(sesion, _pelicula);
            seleccionButacasWindow.ShowDialog();

            // Si completó la reserva, cerrar esta ventana
            if (seleccionButacasWindow.DialogResult == true)
            {
                this.Close();
            }
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}