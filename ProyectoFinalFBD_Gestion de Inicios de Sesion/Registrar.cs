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
        SqlDataAdapter Adaptador = null;
        DataTable Tabla = new DataTable();

        DateTime currentDate;
        int tipoSexo;


        // Constructor
        public Registrar()
        {
            InitializeComponent();
            CargarInfo();

            // Inicializar elementos del comboBox
            cmbSexo.Items.Add("Masculino");
            cmbSexo.Items.Add("Femenino");
            cmbSexo.Items.Add("Otro");
            cmbSexo.DropDownStyle = ComboBoxStyle.DropDownList;
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
                            LimpiarCampos(); ActualizarInfo();

                        }
                        catch (Exception ex) { MessageBox.Show("Error " + ex.Message); }
                    }
                }
                else { MessageBox.Show("¡Edad fuera de rango!"); }
            }
            else { MessageBox.Show("Se requiere llenar todos los campos"); }
        }


        // Procedimiento que muestra inicialmente la información en el DataGridView
        private void CargarInfo()
        {
            // Emplear la conexión de la base de datos
            using (SqlConnection con = BD_Conexion.GetConnection())
            {
                // Consulta que relaciona al DataGridView como destino de los datos
                // Se relaciona las tablas para recuperar datos de Usuarios.SexoID en Sexo.Descripcion
                query = "SELECT Usuarios.ID, Usuarios.Usuario, Usuarios.Contrasena, Usuarios.FechaDeRegistro, Usuarios.CorreoElectronico, Usuarios.Telefono, Usuarios.Nombre, Usuarios.ApellidoP, Usuarios.ApellidoM, Usuarios.Edad," +
                        "Sexo.Descripcion AS Sexo FROM [Usuarios] INNER JOIN Sexo ON Usuarios.SexoID = Sexo.ID";
                Adaptador = new SqlDataAdapter(query, con);
                Adaptador.Fill(Tabla);
                dgvRegistros.DataSource = Tabla;
            }
        }

        // Procedimiento que actualiza la información del DataGridView
        private void ActualizarInfo()
        {
            Tabla.Clear();
            dgvRegistros.ClearSelection();
            using (SqlConnection con = BD_Conexion.GetConnection())
            {
                query = "SELECT Usuarios.ID, Usuarios.Usuario, Usuarios.Contrasena, Usuarios.FechaDeRegistro, Usuarios.CorreoElectronico, Usuarios.Telefono, Usuarios.Nombre, Usuarios.ApellidoP, Usuarios.ApellidoM, Usuarios.Edad," +
                        "Sexo.Descripcion AS Sexo FROM [Usuarios] INNER JOIN Sexo ON Usuarios.SexoID = Sexo.ID";
                Adaptador = new SqlDataAdapter(query, con);
                Adaptador.Fill(Tabla);
                dgvRegistros.DataSource = Tabla;
            }
        }

        // Procedimiento que intenta eliminar un registro
        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            int seleccion = dgvRegistros.CurrentRow.Index; // Guardar el indice seleccionado de la tabla

            // Verificar si hay una fila seleccionada
            if (dgvRegistros.CurrentRow != null && dgvRegistros.CurrentRow.Index > -1)
            {
                DialogResult confirmacion = MessageBox.Show("¿Está seguro que desea eliminar el registro?",
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Usuario confirma la eliminación
                if (confirmacion == DialogResult.Yes)
                {
                    // Emplear la conexión de la base de datos
                    using (SqlConnection con = BD_Conexion.GetConnection())
                    {
                        // Definir la consulta
                        query = "DELETE FROM Usuarios WHERE ID = @Id DELETE FROM IntentosDeSesion WHERE UsuarioID = @Id DELETE FROM EstadoDeLaSesion WHERE UsuarioID = @Id";
                        Comando = new SqlCommand(query, con);
                        Comando.Parameters.AddWithValue("@Id", dgvRegistros.Rows[seleccion].Cells[0].Value);

                        // Realizar la consulta
                        try
                        {
                            Comando.ExecuteNonQuery();
                            MessageBox.Show("Registro Eliminado");
                            ActualizarInfo(); // Reflejar en la tabla la eliminación
                        }
                        catch (Exception ex)
                        { MessageBox.Show("Error " + ex.Message); }
                    }
                }
            }
            else
            { throw new Exception("Seleccione una fila para eliminar."); }
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


        // Procedimiento que maneja la búsqueda al cambiar el texto
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (txtBuscar.Text != "")
            {
                dgvRegistros.CurrentCell = null;

                //Recorrer filas para desaparecer todas
                foreach (DataGridViewRow row in dgvRegistros.Rows)
                { row.Visible = false; }

                //Se recorren las filas para buscar el valor
                foreach (DataGridViewRow row in dgvRegistros.Rows)
                {
                    //Se recorre de celda en celda la fila del foreach anterior
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        //Comparar celda con el textbox de busqueda
                        if (cell.Value.ToString().ToUpper().IndexOf(txtBuscar.Text.ToUpper()) == 0)
                        { row.Visible = true; break; }
                    }
                }
            }
            else { ActualizarInfo(); }
        }

        // Procedimientos que maneja el evento
        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        { if (e.KeyChar == Convert.ToChar(Keys.Enter)) { txtContraseña.Focus(); } }
        private void txtContraseña_KeyPress(object sender, KeyPressEventArgs e)
        { if (e.KeyChar == Convert.ToChar(Keys.Enter)) { txtEmail.Focus(); } }
        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        { if (e.KeyChar == Convert.ToChar(Keys.Enter)) { txtTelefono.Focus(); } }
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

