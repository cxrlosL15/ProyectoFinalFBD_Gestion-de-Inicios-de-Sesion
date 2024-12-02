using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFinalFBD_Gestion_de_Inicios_de_Sesion
{
    public partial class Registrar : Form
    {
        // Variables
        SqlCommand Comando;
        string query = "";

        DateTime currentDate;
        int tipoSexo;


        // Constructor
        public Registrar()
        {
            InitializeComponent();

            // Inicializar elementos del comboBox
            cmbSexo.Items.Add("Masculino");
            cmbSexo.Items.Add("Femenino");
            cmbSexo.Items.Add("Otro");
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.loginEstatico.Show();
        }

        private void Registrar_FormClosing(object sender, FormClosingEventArgs e)   {  }

        // Procedimiento que intenta registrar el usuario
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Comprueba el llenado de la información
            if(ValidarCampoVacio())
            {   
                // Emplear la conexión de la base de datos
                using (SqlConnection con = BD_Conexion.GetConnection())
                {
                    // Definir la consulta
                    query = "INSERT INTO Usuarios(Usuario, Contrasena, FechaDeRegistro, CorreoElectronico, Telefono, Nombre, ApellidoP, ApellidoM, Edad, SexoID) VALUES (@Usuario, @Contrasena, @FechaDeRegistro, @CorreoElectronico, @Telefono, @Nombre, @ApellidoP, @ApellidoM, @Edad, @SexoID)";
                    Comando = new SqlCommand(query, con);

                    Comando.Parameters.AddWithValue("@Usuario", txtUsuario.Text);
                    Comando.Parameters.AddWithValue("@Contrasena", txtContraseña.Text);
                    Comando.Parameters.AddWithValue("@FechaDeRegistro", currentDate = DateTime.Now);
                    Comando.Parameters.AddWithValue("@CorreoElectronico", txtEmail.Text);
                    Comando.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                    Comando.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    Comando.Parameters.AddWithValue("@ApellidoP", txtApellidoP.Text);
                    Comando.Parameters.AddWithValue("@ApellidoM", txtApellidoM.Text);
                    Comando.Parameters.AddWithValue("@Edad", txtEdad.Text);
                    Comando.Parameters.AddWithValue("@SexoID", ObtenerSexo());

                    // Realizar la consulta
                    try
                    {
                        Comando.ExecuteNonQuery();
                        MessageBox.Show("Registro Insertado");
                        LimpiarCampos();

                    }
                    catch (Exception ex) { MessageBox.Show("Error " + ex.Message); }
                }
            }
            else { MessageBox.Show("Se requiere llenar todos los campos"); }
        }

        // Funciones adicionales
        private int ObtenerSexo()
        {
            if (cmbSexo.Text == "Masculino") { tipoSexo = 1; }
            else if (cmbSexo.Text == "Femenino") { tipoSexo = 2; }
            else if (cmbSexo.Text == "Otro") { tipoSexo = 3; }

            return tipoSexo;
        }
        private void LimpiarCampos()
        {
            txtUsuario.Clear();
            txtContraseña.Clear();
            txtEmail.Clear();
            txtTelefono.Clear();
            txtNombre.Clear();
            txtApellidoP.Clear();
            txtApellidoM.Clear();
            txtEdad.Clear();
            cmbSexo.SelectedIndex = -1;
        }
        private bool ValidarCampoVacio()
        {
            return txtUsuario.Text != "" && txtContraseña.Text != "" && txtEmail.Text != "" && txtTelefono.Text != "" && txtNombre.Text != "" && txtApellidoP.Text != "" && txtApellidoM.Text != "" && txtEdad.Text != "" && cmbSexo.SelectedIndex != -1;
        }

    }
}

