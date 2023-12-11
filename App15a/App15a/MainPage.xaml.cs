using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App15a
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnSalir_Clicked(object sender, EventArgs e)
        {
            var respuesta = await DisplayAlert("Confirmación", "¿Seguro que deseas cerrar la aplicación?", "Si", "No");


            if (respuesta)
            {     // Si la Repuesta es que si, Cierra la aplicacion:
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();

            }
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            // Declaramos dos Variables, y guardamos lo que escribe el usuario:
            string usuario = txtUsuario.Text;
            string password = txtPassword.Text;

            // Validamos con la Funcion IsNull, que no esten vacios el Usuario y el Password:
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                // Si los Datos estan vacios muestra este mensaje:
                await DisplayAlert("Error", "Ingrese un usuario y contraseña válidos", "OK");
                return;
            }
            // Establecemos la Cadena de Conexion con el SQL Server y Nuestros datos:
            string connectionString = @"data source=10.0.0.220;initial catalog=LOGIN_PDM;user id=jhoan;password=jhoan;";

            // conectar a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // crear un comando para seleccionar un usuario con el nombre de usuario y contraseña especificados
                using (SqlCommand command = new SqlCommand("SELECT id_usuario, id_rol FROM usuarios WHERE nombre_usuario = @usuario AND password = @password", connection))
                {
                    command.Parameters.AddWithValue("@usuario", usuario);
                    command.Parameters.AddWithValue("@password", password);

                    // ejecutar el comando y recibir el resultado

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        // verificar si se obtuvo algún resultado
                        if (reader.HasRows)
                        {
                            // leer los datos del usuario
                            reader.Read();
                            int idRoles = reader.GetInt32(1);
                            // validar si el usuario es administrador
                            if (idRoles == 1)
                            {  // Si es válido abrir la página de Administrador
                                await Navigation.PushAsync(new Administrador());
                            }
                            else if (idRoles == 2)
                            {// Si es válido abrir la página de Supervisor
                                await Navigation.PushAsync(new Supervisosr());
                            }
                            else if (idRoles == 3)
                            {// Si es válido abrir la página vendedora
                                await Navigation.PushAsync(new Vendedor());
                            }
                            else
                            { // Si no es válido enviar este mensaje de Error:
                                await DisplayAlert("Error", "Usuario o contraseña incorrectos", "Intentar nuevamente");
                            }
                        }
                        else
                        { // Si no es válido enviar este mensaje de Error:
                            await DisplayAlert("Error", "Usuario o contraseña incorrectos", "Intentar nuevamente");
                        }
                    }
                }
            }
        }
    }
}
