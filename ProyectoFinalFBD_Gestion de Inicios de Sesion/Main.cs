using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProyectoFinalFBD_Gestion_de_Inicios_de_Sesion;
using System.Reflection.Emit;

namespace ProyectoFinalFBD_Gestion_de_Inicios_de_Sesion
{
    public partial class Main : Form
    {
        // Variables
        SqlCommand Comando;
        string query = "";

        DateTime currentDate;
        DateTimeOffset lastInputTime;
        string IP = "";
        int ID = 0;



        private static Main instancia = null; //Inicializacion del formulario estatico

        //Método para obtener solamente un formulario abierto de tipo "frmMenu_ESA"
        public static Main ventanaUnica()
        {
            //Evaluar 
            if (instancia == null)
            {
                //Crear uno nuevo
                instancia = new Main();
                return instancia;
            }
            //Regresar el que se creo con anterioridad
            return instancia;
        }

        //Constructor
        public Main()
        {
            InitializeComponent();
            timerInactividad.Start(); // Inicializar temporizador
        }


        private void btnSalir_Click(object sender, EventArgs e)
        {
            ProcesarSalidaDeLaSesion();
            this.Hide();
            Program.loginEstatico.Show();
        }

        // Procedimiento de evento al salir de formulario
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProcesarSalidaDeLaSesion(); 
            instancia = null;
            Application.Exit(); 
        }


        // Procedimiento que agrega "FechaDeSalida" en el registro
        private void ProcesarSalidaDeLaSesion()
        {
            // Emplear la conexión de la base de datos
            using (SqlConnection con = BD_Conexion.GetConnection())
            {
                // Definir la consulta
                query = "UPDATE EstadoDeLaSesion SET FechaDeSalida = @FechaDeSalida WHERE ID = @ID";
                Comando = new SqlCommand(query, con);
                Comando.Parameters.AddWithValue("@FechaDeSalida", currentDate = DateTime.Now);
                Comando.Parameters.AddWithValue("@ID", ID);

                // Realizar la consulta
                try { Comando.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("Error " + ex.Message); }
            }
        }

        // Procedimiento que registra la información relacionada con el estado actual de la sesión
        private void ProcesarEstadoDeSesion()
        {
            // Emplear la conexión de la base de datos
            using (SqlConnection con = BD_Conexion.GetConnection())
            {
                // Definir la consulta
                query = "INSERT INTO EstadoDeLaSesion(UsuarioID, FechaDeEntrada, DireccionIP, SistemaOperativoID) VALUES (@UsuarioID, @FechaDeEntrada, @DireccionIP, @SistemaOperativoID) SELECT SCOPE_IDENTITY()";// 
                Comando = new SqlCommand(query, con);

                Comando.Parameters.AddWithValue("@UsuarioID", DatosInactividad.control);
                Comando.Parameters.AddWithValue("@FechaDeEntrada", currentDate = DateTime.Now);
                Comando.Parameters.AddWithValue("@DireccionIP", ObtenerIP());
                Comando.Parameters.AddWithValue("@SistemaOperativoID", ObtenerSO());

                // Realizar la consulta
                try
                {
                    //Comando.ExecuteNonQuery();
                    object result = Comando.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        ID = Convert.ToInt32(result);
                    }
                    else
                    { MessageBox.Show("Error: Invalid result from SCOPE_IDENTITY()"); }

                    //int userCount = (int)Comando.ExecuteScalar();
                    //ID = Convert.ToInt32(userCount);
                }
                catch (Exception ex) { MessageBox.Show("Error " + ex.Message); }
            }
        }


        // Procedimiento que maneja el evento del temporizador
        private void timerInactividad_Tick(object sender, EventArgs e)
        {
            // Evaluar tiempo de inactividad
            if (DatosInactividad.GetInputIdleTime().TotalSeconds > 3) // 3 segundos
            {   // Si el tiempo es considerable se guarda, de lo contrario lo ignora

                // Emplear la conexión de la base de datos
                using (SqlConnection con = BD_Conexion.GetConnection())
                {
                    // Definir la consulta
                    query = "UPDATE EstadoDeLaSesion SET UltimaActividadRealizada = @UltimaActividadRealizada WHERE ID = @ID";
                    Comando = new SqlCommand(query, con);
                    Comando.Parameters.AddWithValue("@UltimaActividadRealizada", lastInputTime = DatosInactividad.GetLastInputTime());
                    Comando.Parameters.AddWithValue("@ID", ID);

                    // Realizar la consulta
                    try { Comando.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("Error " + ex.Message); }
                }
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


        // Procedimiento que válida la visibilidad del formulario por evento
        private void Main_VisibleChanged(object sender, EventArgs e)
        { 
            // Validar
            if (this.Visible)
            { ProcesarEstadoDeSesion();} // Inicializar proceso
        }
    }
}