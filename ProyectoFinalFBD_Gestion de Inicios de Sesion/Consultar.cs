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
    public partial class Consultar : Form
    {
        // Variables
        SqlCommand Comando;
        string query = "";
        SqlDataAdapter Adaptador = null;
        DataTable Tabla = new DataTable();


        // Constructor
        public Consultar()
        {
            InitializeComponent();
            CargarInfo(); 
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.loginEstatico.Show();
        }

        private void Consultar_FormClosing(object sender, FormClosingEventArgs e) {  }

        // Procedimiento que muestra inicialmente la información en el DataGridView
        private void CargarInfo()
        {
            // Emplear la conexión de la base de datos
            using (SqlConnection con = BD_Conexion.GetConnection())
            {
                // Consulta que relaciona al DataGridView como destino de los datos
                query = "SELECT * FROM Usuarios";
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
                query = "SELECT * FROM Usuarios";
                Adaptador = new SqlDataAdapter(query, con);
                Adaptador.Fill(Tabla);
                dgvRegistros.DataSource = Tabla;
            }
        }

        // En construción...
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
                        { row.Visible = true;     break; }
                    }
                }
            }
        }

        // Procedimiento que intenta eliminar un registro
        private void btnEliminar_Click(object sender, EventArgs e)
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
                        {  MessageBox.Show("Error " + ex.Message); }
                    }
                }
            }
            else
            { throw new Exception("Seleccione una fila para eliminar.");  }
        }
    }
}
