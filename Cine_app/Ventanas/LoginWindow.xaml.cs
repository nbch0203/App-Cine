using System;
using System.Windows;
using Cine_app.Modelos;
using Cine_app.Servicios;

namespace Cine_app.Ventanas
{
    public partial class LoginWindow : Window
    {
        private readonly ServicioBaseDeDatos _dbService;
        public Usuario? UsuarioAutenticado { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
            _dbService = new ServicioBaseDeDatos();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Ocultar mensaje de error previo
            txtMensaje.Visibility = Visibility.Collapsed;
            txtMensaje.Text = "";

            // Validar campos vacios
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MostrarError("Por favor, ingrese su email");
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MostrarError("Por favor, ingrese su contrasena");
                txtPassword.Focus();
                return;
            }

            // Deshabilitar boton mientras se valida
            btnLogin.IsEnabled = false;
            btnLogin.Content = "Validando...";

            try
            {
                // Validar usuario
                UsuarioAutenticado = await _dbService.ValidarUsuarioAsync(
                    txtEmail.Text.Trim(), 
                    txtPassword.Password
                );

                if (UsuarioAutenticado != null)
                {
                    // Login exitoso - guardar en el servicio de sesion
                    ServicioSesion.Instance.IniciarSesion(UsuarioAutenticado);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    // Credenciales incorrectas
                    MostrarError("Email o contrasena incorrectos");
                    txtPassword.Password = "";
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al conectar con la base de datos: {ex.Message}");
            }
            finally
            {
                // Rehabilitar boton
                btnLogin.IsEnabled = true;
                btnLogin.Content = "Iniciar Sesion";
            }
        }

        private void BtnRegistro_Click(object sender, RoutedEventArgs e)
        {
            // Abrir ventana de registro
            var registroWindow = new RegistroWindow();
            bool? resultado = registroWindow.ShowDialog();

            // Si el registro fue exitoso, mostrar mensaje
            if (resultado == true)
            {
                txtMensaje.Text = "Registro completado. Por favor, inicie sesion con sus credenciales.";
                txtMensaje.Foreground = System.Windows.Media.Brushes.Green;
                txtMensaje.Visibility = Visibility.Visible;
                
                // Limpiar campos
                txtEmail.Text = "";
                txtPassword.Password = "";
                txtEmail.Focus();
            }
        }

        private void MostrarError(string mensaje)
        {
            txtMensaje.Text = mensaje;
            txtMensaje.Visibility = Visibility.Visible;
        }

        // Permitir login con Enter
        private void TxtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                BtnLogin_Click(sender, e);
            }
        }
    }
}