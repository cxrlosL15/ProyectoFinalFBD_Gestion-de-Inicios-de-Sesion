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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProyectoFinalFBD_Gestion_de_Inicios_de_Sesion
{
    public partial class Registrar : Form
    {
        // Variables
        SqlCommand Comando;
        string query = "";

        DateTime currentDate;
        int tipoSexo;



        private static Registrar instancia = null; //Inicializacion del formulario estatico

        //Método para obtener solamente un formulario abierto de tipo "frmMenu_ESA"
        public static Registrar ventanaUnica()
        {
            //Evaluar 
            if (instancia == null)
            {
                //Crear uno nuevo
                instancia = new Registrar();
                return instancia;
            }
            //Regresar el que se creo con anterioridad
            return instancia;
        }

        // Constructor
        public Registrar()
        {
            InitializeComponent();

            // Inicializar elementos del comboBox
            cmbSexo.Items.Add("Masculino");
            cmbSexo.Items.Add("Femenino");
            cmbSexo.Items.Add("Otro");
            cmbSexo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Hide();
            Program.loginEstatico.Show();
        }

        private void Registrar_FormClosing(object sender, FormClosingEventArgs e)   { instancia = null; Application.Exit(); }


        // Procedimiento que intenta registrar el usuario
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Comprueba el llenado de la información
            if(ValidarCampoVacio())
            {   // Comprueba la integridad de la información
                if (Validaciones())
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
                else { MessageBox.Show("¡Edad fuera de rango!"); }
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
        private bool Validaciones()
        {
            int x = Convert.ToInt32(txtEdad.Text);

            return x >= 18 && x <= 100;
        }


        // Procedimientos que maneja el evento
        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        { if (e.KeyChar == Convert.ToChar(Keys.Enter)) { txtContraseña.Focus(); } }
        private void txtContraseña_KeyPress(object sender, KeyPressEventArgs e)
        { if (e.KeyChar == Convert.ToChar(Keys.Enter)) { txtEmail.Focus(); } }
        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        { if (e.KeyChar == Convert.ToChar(Keys.Enter)) { txtTelefono.Focus(); } }
        // CONSIDERAR TIPO DE DATO EN BD
        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        { 
            if (e.KeyChar == Convert.ToChar(Keys.Enter)) { txtNombre.Focus(); }
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se admiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true; return;
            }
        }
        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        { 
            if (e.KeyChar == Convert.ToChar(Keys.Enter)) { txtApellidoM.Focus(); }
            if ((e.KeyChar >= 32 && e.KeyChar <= 64) || (e.KeyChar >= 91 && e.KeyChar <= 96) || (e.KeyChar >= 123 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se admiten letras", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true; return;
            }
        }
        private void txtApellidoM_KeyPress(object sender, KeyPressEventArgs e)
        { 
            if (e.KeyChar == Convert.ToChar(Keys.Enter)) { txtApellidoP.Focus(); }
            if ((e.KeyChar >= 32 && e.KeyChar <= 64) || (e.KeyChar >= 91 && e.KeyChar <= 96) || (e.KeyChar >= 123 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se admiten letras", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true; return;
            }
        }
        private void txtApellidoP_KeyPress(object sender, KeyPressEventArgs e)
        { 
            if (e.KeyChar == Convert.ToChar(Keys.Enter)) { txtEdad.Focus(); }
            if ((e.KeyChar >= 32 && e.KeyChar <= 64) || (e.KeyChar >= 91 && e.KeyChar <= 96) || (e.KeyChar >= 123 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se admiten letras", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true; return;
            }
        }
        private void txtEdad_KeyPress(object sender, KeyPressEventArgs e)
        { 
            if (e.KeyChar == Convert.ToChar(Keys.Enter)) { cmbSexo.Focus(); }
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se admiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true; return;
            }
        }

    }
}

