using System.Windows;
using Cine_app.Modelos;
using Cine_app.Vistas;
using Cine_app.Ventanas;

namespace Cine_app
{
    public partial class MainWindow : Window
    {
        private CarteleraView? _carteleraView;
        private SeleccionSesionView? _seleccionSesionView;
        private SeleccionButacasWindow? _seleccionButacasWindow;

        public MainWindow()
        {
            InitializeComponent();
            MostrarCartelera();
        }

        private void MostrarCartelera()
        {
            _carteleraView = new CarteleraView();
            _carteleraView.PeliculaSeleccionada += OnPeliculaSeleccionada;
            contentMain.Content = _carteleraView;
        }

        private void OnPeliculaSeleccionada(object? sender, Pelicula pelicula)
        {
            MostrarSeleccionSesion(pelicula);
        }

        private void MostrarSeleccionSesion(Pelicula pelicula)
        {
            _seleccionSesionView = new SeleccionSesionView();
            _seleccionSesionView.Volver += (s, e) => MostrarCartelera();
            _seleccionSesionView.SesionSeleccionada += OnSesionSeleccionada;
            _seleccionSesionView.CargarPelicula(pelicula);
            contentMain.Content = _seleccionSesionView;
        }

        private void OnSesionSeleccionada(object? sender, (Sesion sesion, Pelicula pelicula) data)
        {
            MostrarSeleccionButacas(data.sesion, data.pelicula);
        }

        private void MostrarSeleccionButacas(Sesion sesion, Pelicula pelicula)
        {
            // La ventana de selección de butacas sigue siendo una ventana modal por su complejidad
            // Pero ahora se abre desde la misma ventana principal
            _seleccionButacasWindow = new SeleccionButacasWindow(sesion, pelicula);
            _seleccionButacasWindow.Owner = this; // Establecer como propietaria esta ventana
            
            var result = _seleccionButacasWindow.ShowDialog();
            
            // Después de cerrar la ventana de butacas, volver a la cartelera
            if (result == true)
            {
                MessageBox.Show("¡Reserva completada exitosamente!\n\nGracias por su compra.",
                              "Reserva Exitosa",
                              MessageBoxButton.OK,
                              MessageBoxImage.Information);
            }
            
            MostrarCartelera();
        }
    }
}
