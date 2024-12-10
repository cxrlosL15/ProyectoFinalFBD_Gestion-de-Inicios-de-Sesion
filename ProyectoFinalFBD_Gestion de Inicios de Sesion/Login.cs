using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFinalFBD_Gestion_de_Inicios_de_Sesion
{
    public partial class Login : Form
    {
        // Variables
        SqlCommand Comando;
        string query = "";

        DateTime currentDate;
        string IP = "";
        int usuarioID = 0;
        bool seePassword = false;


        // Constructor
        public Login()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            txtContraseña.Clear(); txtUsuario.Clear();

            Consultar form = Consultar.ventanaUnica(); // Validar estado de la instancia
            Program.loginEstatico.Hide();
            form.Show();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            txtContraseña.Clear(); txtUsuario.Clear();

            Registrar form = Registrar.ventanaUnica(); // Validar estado de la instancia
            Program.loginEstatico.Hide();
            form.Show();
        }


        // Procedimiento que intenta entrar al programa principal
        private void btnEntrar_Click(object sender, EventArgs e)
        {
            // Validar campos vacios
            if (txtContraseña.Text != "" && txtUsuario.Text != "")
            {
                //Asignación
                string username = txtUsuario.Text, password = txtContraseña.Text;

                // Comprobar el inicio de sesión
                if (ValidarLogin(username, password)) //CORRECTO
                {
                    // Recuperar ID del usuario para procesarlo
                    usuarioID = (int)ObtenerID(username);
                    DatosInactividad.control = usuarioID;  // Indicador

                    ProcesarIntentoDeSesion(1); // Almacenar el resultado del intento

                    txtContraseña.Text = ""; txtUsuario.Text = "";

                    // Ingreso al programa principal
                    Main form = Main.ventanaUnica();
                    Program.loginEstatico.Hide();
                    form.Show();
                }
                else //INCORRECTO 
                {
                    // Si es válido el username pero la contraseña no,
                    // se le relaciona con un fracaso de intento
                    if (ValidarLoginIncorrecto(username))
                    {
                        // Recuperar ID del usuario para procesarlo
                        usuarioID = (int)ObtenerID(username);
                        ProcesarIntentoDeSesion(0); // Almacenar el resultado del intento
                    } 
                    MessageBox.Show("Nombre de usuario o contraseña incorrectos.");
                }
            }
            else { MessageBox.Show("Se requiere llenar los campos"); }
        }



        // Procedimiento que agrega el éxito/fracaso del intento por el usuario
        private void ProcesarIntentoDeSesion(int estado)
        {
            // Emplear la conexión de la base de datos
            using (SqlConnection con = BD_Conexion.GetConnection())
            {
                // Definir la consulta
                query = "INSERT INTO IntentosDeSesion(UsuarioID, Estado, DireccionIP, SistemaOperativoID, Fecha) VALUES (@UsuarioID, @Estado, @DireccionIP, @SistemaOperativoID, @Fecha)";
                Comando = new SqlCommand(query, con);

                Comando.Parameters.AddWithValue("@UsuarioID", usuarioID);
                Comando.Parameters.AddWithValue("@Estado", estado);
                Comando.Parameters.AddWithValue("@DireccionIP", ObtenerIP());
                Comando.Parameters.AddWithValue("@SistemaOperativoID", ObtenerSO());
                Comando.Parameters.AddWithValue("@Fecha", currentDate = DateTime.Now);

                // Realiza la consulta 
                try { Comando.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("Error " + ex.Message); }
            }
        }


        // Función que recupera el ID del usuario
        private int? ObtenerID(string username) // Acepta devolver valor nulo
        {
            // Emplear la conexión de la base de datos
            using (SqlConnection con = BD_Conexion.GetConnection())
            {
                // Definir la consulta
                query = "SELECT ID FROM Usuarios WHERE Usuario = @Username";
                Comando = new SqlCommand(query, con);
                Comando.Parameters.AddWithValue("@Username", username);

                // Realiza la consulta y se guarda el objeto devuelto
                object result = Comando.ExecuteScalar();

                // Comprueba si es válido
                if (result != null && result != DBNull.Value)
                {
                    return (int)result; // Devuelve el valor entero del ID
                }
                else { return null; }
            }
        }


        // Función que intenta encontrar al usuario y contraseña en los registros
        private bool ValidarLogin(string username, string password)
        {
            // Emplear la conexión de la base de datos
            using (SqlConnection con = BD_Conexion.GetConnection())
            {
                // Definir la consulta
                query = "SELECT COUNT(*) FROM Usuarios WHERE Usuario = @Username AND Contrasena = @Password";
                Comando = new SqlCommand(query, con);
                Comando.Parameters.AddWithValue("@Username", username);
                Comando.Parameters.AddWithValue("@Password", password);

                // Realiza la consulta y devuelve el numero de coincidencias(fila encontrada)
                int userCount = (int)Comando.ExecuteScalar();
                usuarioID = Convert.ToInt32(userCount);

                return userCount > 0; //Retornar el resultado de la comparación
            }
        }
    
        // Función que intenta encuentrar al usuario pero la contraseña es incorrecta
        private bool ValidarLoginIncorrecto(string username)
        {
            // Emplear la conexión de la base de datos
            using (SqlConnection con = BD_Conexion.GetConnection())
            {
                // Definir la consulta
                query = "SELECT COUNT(*) FROM Usuarios WHERE Usuario = @Username";
                Comando = new SqlCommand(query, con);
                Comando.Parameters.AddWithValue("@Username", username);

                // Realiza la consulta y devuelve el numero de coincidencias(fila encontrada)
                int userCount = (int)Comando.ExecuteScalar();
                usuarioID = Convert.ToInt32(userCount);

                return userCount > 0; //Retornar el resultado de la comparación
            }
        }

        // Funciones adicionales
        private string ObtenerIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                { IP = ip.ToString(); break; }
            }

            return IP;
        }
        private int? ObtenerSO()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) { return 1; }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) { return 2; }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) { return 3; }
            else { return null; }
        }


        // Controla la visibilidad de la contraseña
        private void pbPassword_Click(object sender, EventArgs e)
        {   // Invertir estado
            seePassword = !seePassword;

            if (seePassword) // Mirar contraseña
            {
                txtContraseña.PasswordChar = '\0'; // Modificar propiedad
                pbPassword.Image = ProyectoFinalFBD_Gestion_de_Inicios_de_Sesion.Properties.Resources.hide; // Asignar imagen correspondiente
            }
            else // Ocultar contraseña
            {
                txtContraseña.PasswordChar = '*';  // Modificar propiedad
                pbPassword.Image = ProyectoFinalFBD_Gestion_de_Inicios_de_Sesion.Properties.Resources.show; // Asignar imagen correspondiente
            }
        }

        // Procedimientos que maneja el evento
        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        { if(e.KeyChar == Convert.ToChar(Keys.Enter)) { txtContraseña.Focus(); } }

        private void txtContraseña_KeyPress(object sender, KeyPressEventArgs e)
        { if (e.KeyChar == Convert.ToChar(Keys.Enter)) { btnEntrar.Focus(); } }

    }
}