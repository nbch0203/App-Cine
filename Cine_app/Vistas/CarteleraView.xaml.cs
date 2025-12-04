using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Cine_app.Modelos;
using Cine_app.Servicios;

namespace Cine_app.Vistas
{
    public partial class CarteleraView : UserControl
    {
        private readonly ServicioBaseDeDatos _dbService;
        public event EventHandler<Pelicula>? PeliculaSeleccionada;

        public CarteleraView()
        {
            InitializeComponent();
            _dbService = new ServicioBaseDeDatos();
            
            Loaded += async (s, e) => await CargarPeliculas();
            
            // Suscribirse a eventos de sesión
            ServicioSesion.Instance.SesionIniciada += (s, e) => ActualizarEstadoUsuario();
            ServicioSesion.Instance.SesionCerrada += (s, e) => ActualizarEstadoUsuario();
            
            ActualizarEstadoUsuario();
        }

        private void ActualizarEstadoUsuario()
        {
            if (ServicioSesion.Instance.EstaAutenticado)
            {
                txtBienvenida.Text = $"Bienvenido, {ServicioSesion.Instance.UsuarioActual?.Nombre}";
                btnCuentaAccion.Content = "Cerrar Sesion";
                btnPerfil.Visibility = Visibility.Visible;
            }
            else
            {
                txtBienvenida.Text = "Bienvenido, Invitado";
                btnCuentaAccion.Content = "Iniciar Sesion";
                btnPerfil.Visibility = Visibility.Collapsed;
            }
        }

        private async Task CargarPeliculas()
        {
            try
            {
                pnlLoading.Visibility = Visibility.Visible;
                itemsPeliculas.Visibility = Visibility.Collapsed;

                var peliculas = await _dbService.ObtenerPeliculasActivasAsync();

                if (peliculas.Count == 0)
                {
                    MessageBox.Show("No hay peliculas disponibles en este momento.",
                                  "Sin peliculas",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                    return;
                }

                var items = new List<UIElement>();
                foreach (var pelicula in peliculas)
                {
                    var card = CrearCardPelicula(pelicula);
                    items.Add(card);
                }

                itemsPeliculas.ItemsSource = items;
                itemsPeliculas.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las peliculas: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
            finally
            {
                pnlLoading.Visibility = Visibility.Collapsed;
            }
        }

        private Border CrearCardPelicula(Pelicula pelicula)
        {
            var border = new Border
            {
                Width = 250,
                Margin = new Thickness(15),
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            border.Effect = new System.Windows.Media.Effects.DropShadowEffect
            {
                BlurRadius = 10,
                ShadowDepth = 3,
                Opacity = 0.3
            };

            var stackPanel = new StackPanel();

            // Imagen
            var imagen = new Image
            {
                Height = 350,
                Stretch = Stretch.UniformToFill
            };

            if (!string.IsNullOrEmpty(pelicula.ImagenUrl))
            {
                try
                {
                    imagen.Source = new BitmapImage(new Uri(pelicula.ImagenUrl));
                }
                catch
                {
                    imagen.Source = null;
                }
            }

            var borderImagen = new Border
            {
                Child = imagen,
                CornerRadius = new CornerRadius(10, 10, 0, 0),
                ClipToBounds = true
            };

            stackPanel.Children.Add(borderImagen);

            // Información
            var infoPadding = new Border { Padding = new Thickness(15) };
            var infoStack = new StackPanel();

            var titulo = new TextBlock
            {
                Text = pelicula.Titulo,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var genero = new TextBlock
            {
                Text = $"Genero: {pelicula.Genero ?? "N/A"}",
                FontSize = 14,
                Foreground = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66)),
                Margin = new Thickness(0, 0, 0, 5)
            };

            var duracion = new TextBlock
            {
                Text = $"Duracion: {(pelicula.Duracion.HasValue ? $"{pelicula.Duracion} min" : "N/A")}",
                FontSize = 14,
                Foreground = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66)),
                Margin = new Thickness(0, 0, 0, 15)
            };

            var btnVerHorarios = new Button
            {
                Content = "Ver Horarios",
                Background = new SolidColorBrush(Color.FromRgb(0xe9, 0x45, 0x60)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(15, 10, 15, 10),
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Cursor = System.Windows.Input.Cursors.Hand,
                Tag = pelicula
            };

            btnVerHorarios.Click += (s, e) =>
            {
                PeliculaSeleccionada?.Invoke(this, pelicula);
            };

            infoStack.Children.Add(titulo);
            infoStack.Children.Add(genero);
            infoStack.Children.Add(duracion);
            infoStack.Children.Add(btnVerHorarios);

            infoPadding.Child = infoStack;
            stackPanel.Children.Add(infoPadding);
            border.Child = stackPanel;

            return border;
        }

        private void BtnCuentaAccion_Click(object sender, RoutedEventArgs e)
        {
            if (ServicioSesion.Instance.EstaAutenticado)
            {
                // Cerrar sesión
                var resultado = MessageBox.Show("¿Desea cerrar sesion?",
                                              "Cerrar Sesion",
                                              MessageBoxButton.YesNo,
                                              MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    ServicioSesion.Instance.CerrarSesion();
                    ActualizarEstadoUsuario();
                }
            }
            else
            {
                // Abrir login
                var loginWindow = new Ventanas.LoginWindow();
                loginWindow.ShowDialog();
                ActualizarEstadoUsuario();
            }
        }

        private void BtnPerfil_Click(object sender, RoutedEventArgs e)
        {
            var perfilWindow = new Ventanas.PerfilUsuarioWindow();
            perfilWindow.ShowDialog();
        }
    }
}
