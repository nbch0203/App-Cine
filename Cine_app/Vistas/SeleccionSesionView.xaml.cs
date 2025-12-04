using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Cine_app.Modelos;
using Cine_app.Servicios;

namespace Cine_app.Vistas
{
    public partial class SeleccionSesionView : UserControl
    {
        private readonly ServicioBaseDeDatos _dbService;
        private Pelicula? _pelicula;

        public event EventHandler? Volver;
        public event EventHandler<(Sesion sesion, Pelicula pelicula)>? SesionSeleccionada;

        public SeleccionSesionView()
        {
            InitializeComponent();
            _dbService = new ServicioBaseDeDatos();

            // Configurar calendario
            calendario.DisplayDateStart = DateTime.Today;
            calendario.DisplayDateEnd = DateTime.Today.AddMonths(2);
            calendario.SelectedDate = DateTime.Today;
        }

        public void CargarPelicula(Pelicula pelicula)
        {
            _pelicula = pelicula;

            txtTitulo.Text = pelicula.Titulo;

            var info = new List<string>();
            if (!string.IsNullOrEmpty(pelicula.Director))
                info.Add($"Director: {pelicula.Director}");
            if (pelicula.Duracion.HasValue)
                info.Add($"Duracion: {pelicula.Duracion} min");
            if (!string.IsNullOrEmpty(pelicula.Genero))
                info.Add($"Genero: {pelicula.Genero}");

            txtInfo.Text = string.Join("\n", info);

            // Cargar imagen
            if (!string.IsNullOrEmpty(pelicula.ImagenUrl))
            {
                try
                {
                    imgPelicula.Source = new BitmapImage(new Uri(pelicula.ImagenUrl));
                }
                catch { }
            }

            // Cargar sesiones de hoy
            _ = CargarSesiones(DateTime.Today);
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            Volver?.Invoke(this, EventArgs.Empty);
        }

        private async void Calendario_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calendario.SelectedDate.HasValue)
            {
                await CargarSesiones(calendario.SelectedDate.Value);
            }
        }

        private async Task CargarSesiones(DateTime fecha)
        {
            if (_pelicula == null) return;

            try
            {
                pnlLoadingSesiones.Visibility = Visibility.Visible;
                scrollSesiones.Visibility = Visibility.Collapsed;
                pnlSinSesiones.Visibility = Visibility.Collapsed;

                var sesiones = await _dbService.ObtenerSesionesPorPeliculaAsync(_pelicula.Id, fecha);

                if (sesiones == null || !sesiones.Any())
                {
                    pnlSinSesiones.Visibility = Visibility.Visible;
                    return;
                }

                var items = new List<UIElement>();
                foreach (var sesion in sesiones)
                {
                    var card = CrearCardSesion(sesion);
                    items.Add(card);
                }

                itemsSesiones.ItemsSource = items;
                scrollSesiones.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar sesiones: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
            finally
            {
                pnlLoadingSesiones.Visibility = Visibility.Collapsed;
            }
        }

        private Border CrearCardSesion(Sesion sesion)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0xF9, 0xF9, 0xF9)),
                Padding = new Thickness(20),
                Margin = new Thickness(0, 0, 0, 15),
                CornerRadius = new CornerRadius(8),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0)),
                BorderThickness = new Thickness(1)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var infoStack = new StackPanel();

            // Hora
            var hora = new TextBlock
            {
                Text = sesion.FechaHora.ToString("HH:mm"),
                FontSize = 28,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            };

            // Sala
            var sala = new TextBlock
            {
                Text = $"Sala {sesion.Sala?.Nombre ?? "N/A"} - Estándar",
                FontSize = 16,
                Foreground = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66)),
                Margin = new Thickness(0, 0, 0, 5)
            };

            // Precio
            var precio = new TextBlock
            {
                Text = $"Precio: {sesion.Precio:F2} EUR",
                FontSize = 16,
                Foreground = new SolidColorBrush(Color.FromRgb(0xE9, 0x45, 0x60)),
                FontWeight = FontWeights.SemiBold
            };

            infoStack.Children.Add(hora);
            infoStack.Children.Add(sala);
            infoStack.Children.Add(precio);

            Grid.SetColumn(infoStack, 0);
            grid.Children.Add(infoStack);

            // Botón
            var btnSeleccionar = new Button
            {
                Content = "Seleccionar Asientos",
                Background = new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(20, 12, 20, 12),
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Cursor = System.Windows.Input.Cursors.Hand,
                VerticalAlignment = VerticalAlignment.Center,
                Tag = sesion
            };

            btnSeleccionar.Click += (s, e) =>
            {
                if (!ServicioSesion.Instance.EstaAutenticado)
                {
                    MessageBox.Show("Debe iniciar sesion para realizar una reserva",
                                  "Autenticacion requerida",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);

                    var loginWindow = new Ventanas.LoginWindow();
                    if (loginWindow.ShowDialog() != true)
                        return;
                }

                if (_pelicula != null)
                {
                    SesionSeleccionada?.Invoke(this, (sesion, _pelicula));
                }
            };

            Grid.SetColumn(btnSeleccionar, 1);
            grid.Children.Add(btnSeleccionar);

            border.Child = grid;
            return border;
        }
    }
}
