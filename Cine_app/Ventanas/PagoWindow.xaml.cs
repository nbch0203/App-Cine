using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Cine_app.Ventanas
{
    public partial class PagoWindow : Window
    {
        private readonly decimal _montoTotal;
        private string _metodoPagoSeleccionado = "";

        public bool PagoExitoso { get; private set; } = false;

        public PagoWindow(decimal montoTotal)
        {
            InitializeComponent();
            _montoTotal = montoTotal;
            txtTotal.Text = $"Total: {montoTotal:F2} EUR";
        }

        // ???????????????????????????????????????????????????????????
        // VALIDACIONES DE ENTRADA (SOLO NÚMEROS)
        // ???????????????????????????????????????????????????????????

        private void TxtNumeroTarjeta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Solo permitir números
            e.Handled = !SoloNumeros(e.Text);
        }

        private void TxtFechaExpiracion_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Solo permitir números y "/"
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9/]$");
        }

        private void TxtFechaExpiracion_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Auto-formatear la fecha (agregar / automáticamente)
            var texto = txtFechaExpiracion.Text.Replace("/", "");
            
            if (texto.Length >= 2)
            {
                txtFechaExpiracion.TextChanged -= TxtFechaExpiracion_TextChanged;
                txtFechaExpiracion.Text = texto.Substring(0, 2) + "/" + texto.Substring(2);
                txtFechaExpiracion.CaretIndex = txtFechaExpiracion.Text.Length;
                txtFechaExpiracion.TextChanged += TxtFechaExpiracion_TextChanged;
            }
        }

        private void TxtTelefonoBizum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Solo permitir números
            e.Handled = !SoloNumeros(e.Text);
        }

        private bool SoloNumeros(string texto)
        {
            return Regex.IsMatch(texto, @"^[0-9]+$");
        }

        // ???????????????????????????????????????????????????????????
        // MÉTODOS DE SELECCIÓN DE MÉTODO DE PAGO
        // ???????????????????????????????????????????????????????????

        private void BtnMetodoTarjeta_Click(object sender, RoutedEventArgs e)
        {
            SeleccionarMetodoPago("Tarjeta");
            MostrarFormulario(pnlFormularioTarjeta);
            ResaltarBoton(btnMetodoTarjeta);
        }

        private void BtnMetodoBizum_Click(object sender, RoutedEventArgs e)
        {
            SeleccionarMetodoPago("Bizum");
            MostrarFormulario(pnlFormularioBizum);
            ResaltarBoton(btnMetodoBizum);
        }

        private void BtnMetodoPaypal_Click(object sender, RoutedEventArgs e)
        {
            SeleccionarMetodoPago("PayPal");
            MostrarFormulario(pnlFormularioPaypal);
            ResaltarBoton(btnMetodoPaypal);
        }

        private void SeleccionarMetodoPago(string metodo)
        {
            _metodoPagoSeleccionado = metodo;
            btnPagar.IsEnabled = true;
            OcultarError();
        }

        private void MostrarFormulario(System.Windows.Controls.Border formulario)
        {
            // Ocultar todos los formularios
            pnlFormularioTarjeta.Visibility = Visibility.Collapsed;
            pnlFormularioBizum.Visibility = Visibility.Collapsed;
            pnlFormularioPaypal.Visibility = Visibility.Collapsed;

            // Mostrar el formulario seleccionado
            formulario.Visibility = Visibility.Visible;
        }

        private void ResaltarBoton(System.Windows.Controls.Button boton)
        {
            // Restaurar estilo de todos los botones
            btnMetodoTarjeta.BorderBrush = new SolidColorBrush(Color.FromRgb(0x55, 0x55, 0x55));
            btnMetodoBizum.BorderBrush = new SolidColorBrush(Color.FromRgb(0x55, 0x55, 0x55));
            btnMetodoPaypal.BorderBrush = new SolidColorBrush(Color.FromRgb(0x55, 0x55, 0x55));

            // Resaltar el botón seleccionado
            boton.BorderBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x6b, 0x35));
            boton.BorderThickness = new Thickness(3);
        }

        // ???????????????????????????????????????????????????????????
        // VALIDACIONES DE FORMULARIOS
        // ???????????????????????????????????????????????????????????

        private bool ValidarFormularioTarjeta()
        {
            // Validar número de tarjeta (16 dígitos)
            if (string.IsNullOrWhiteSpace(txtNumeroTarjeta.Text))
            {
                MostrarError("Por favor, ingresa el número de tarjeta.");
                return false;
            }

            if (!Regex.IsMatch(txtNumeroTarjeta.Text, @"^\d{16}$"))
            {
                MostrarError("El número de tarjeta debe tener exactamente 16 dígitos.");
                return false;
            }

            // Validar nombre del titular
            if (string.IsNullOrWhiteSpace(txtNombreTarjeta.Text) || txtNombreTarjeta.Text.Length < 3)
            {
                MostrarError("Por favor, ingresa el nombre del titular (mínimo 3 caracteres).");
                return false;
            }

            // Validar fecha de expiración (MM/AA)
            if (string.IsNullOrWhiteSpace(txtFechaExpiracion.Text))
            {
                MostrarError("Por favor, ingresa la fecha de expiración.");
                return false;
            }

            if (!Regex.IsMatch(txtFechaExpiracion.Text, @"^(0[1-9]|1[0-2])\/\d{2}$"))
            {
                MostrarError("La fecha de expiración debe tener el formato MM/AA (ejemplo: 12/25).");
                return false;
            }

            // Validar que la fecha no esté vencida
            var partes = txtFechaExpiracion.Text.Split('/');
            int mes = int.Parse(partes[0]);
            int anio = int.Parse(partes[1]) + 2000; // Convertir AA a AAAA

            var fechaExpiracion = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes));
            if (fechaExpiracion < DateTime.Now)
            {
                MostrarError("La tarjeta está vencida. Por favor, usa una tarjeta válida.");
                return false;
            }

            // Validar CVV (3 o 4 dígitos)
            if (string.IsNullOrWhiteSpace(txtCVV.Password))
            {
                MostrarError("Por favor, ingresa el código CVV.");
                return false;
            }

            if (!Regex.IsMatch(txtCVV.Password, @"^\d{3,4}$"))
            {
                MostrarError("El CVV debe tener 3 o 4 dígitos.");
                return false;
            }

            return true;
        }

        private bool ValidarFormularioBizum()
        {
            // Validar número de teléfono (9 dígitos)
            if (string.IsNullOrWhiteSpace(txtTelefonoBizum.Text))
            {
                MostrarError("Por favor, ingresa tu número de teléfono.");
                return false;
            }

            // Validar que sean 9 dígitos y que empiece por 6, 7 o 9 (números españoles)
            if (!Regex.IsMatch(txtTelefonoBizum.Text, @"^[679]\d{8}$"))
            {
                MostrarError("El número de teléfono debe tener 9 dígitos y comenzar por 6, 7 o 9.");
                return false;
            }

            return true;
        }

        private bool ValidarFormularioPaypal()
        {
            // Validar email
            if (string.IsNullOrWhiteSpace(txtEmailPaypal.Text))
            {
                MostrarError("Por favor, ingresa tu correo electrónico de PayPal.");
                return false;
            }

            // Validar formato de email
            if (!Regex.IsMatch(txtEmailPaypal.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MostrarError("El correo electrónico no es válido. Debe tener el formato: ejemplo@correo.com");
                return false;
            }

            // Validar contraseña
            if (string.IsNullOrWhiteSpace(txtPasswordPaypal.Password))
            {
                MostrarError("Por favor, ingresa tu contraseña de PayPal.");
                return false;
            }

            if (txtPasswordPaypal.Password.Length < 6)
            {
                MostrarError("La contraseña debe tener al menos 6 caracteres.");
                return false;
            }

            return true;
        }

        // ???????????????????????????????????????????????????????????
        // PROCESAMIENTO DE PAGO
        // ???????????????????????????????????????????????????????????

        private async void BtnPagar_Click(object sender, RoutedEventArgs e)
        {
            // Validar según el método de pago seleccionado
            bool esValido = _metodoPagoSeleccionado switch
            {
                "Tarjeta" => ValidarFormularioTarjeta(),
                "Bizum" => ValidarFormularioBizum(),
                "PayPal" => ValidarFormularioPaypal(),
                _ => false
            };

            if (!esValido)
                return;

            // Deshabilitar botón y mostrar estado de procesamiento
            btnPagar.IsEnabled = false;
            btnPagar.Content = "Procesando...";

            try
            {
                // Simular procesamiento del pago (esperar 2 segundos)
                await System.Threading.Tasks.Task.Delay(2000);

                // Pago exitoso
                PagoExitoso = true;

                MessageBox.Show(
                    $"Pago procesado correctamente!\n\n" +
                    $"Metodo de pago: {_metodoPagoSeleccionado}\n" +
                    $"Monto: {_montoTotal:F2} EUR\n\n" +
                    $"Recibiras un correo de confirmacion con los detalles de tu reserva.",
                    "Pago Exitoso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MostrarError($"Error al procesar el pago: {ex.Message}");
                btnPagar.IsEnabled = true;
                btnPagar.Content = "Pagar";
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            var resultado = MessageBox.Show(
                "Estas seguro de que deseas cancelar el pago?\n\nLa reserva no se completara.",
                "Cancelar Pago",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                PagoExitoso = false;
                DialogResult = false;
                Close();
            }
        }

        // ???????????????????????????????????????????????????????????
        // MÉTODOS AUXILIARES
        // ???????????????????????????????????????????????????????????

        private void MostrarError(string mensaje)
        {
            txtError.Text = mensaje;
            pnlError.Visibility = Visibility.Visible;
        }

        private void OcultarError()
        {
            pnlError.Visibility = Visibility.Collapsed;
        }
    }
}
