using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ProyectoFinalFBD_Gestion_de_Inicios_de_Sesion
{
    public partial class Consultar : Form
    {
        // Variables
        SqlCommand Comando;
        string query = "";
        SqlDataAdapter Adaptador = null;
        DataTable Tabla = new DataTable();
        int Index = 0, indexMax, secuencia=1;



        private static Consultar instancia = null; //Inicializacion del formulario estatico

        //Método para obtener solamente un formulario abierto de tipo "frmMenu_ESA"
        public static Consultar ventanaUnica()
        {
            //Evaluar 
            if (instancia == null)
            {
                //Crear uno nuevo
                instancia = new Consultar();
                return instancia;
            }
            //Regresar el que se creo con anterioridad
            return instancia;
        }

        // Constructor
        public Consultar()
        {
            InitializeComponent();
            CargarInfo();

            // Comprobar si tiene datos por mostrar
            if (dgvRegistros.Rows.Count == 0) // VACÍO
            { MessageBox.Show("No hay datos por mostrar. ¡Intente registrar usuarios!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            else                            // NO VACÍO
            {  LlenarPanel(); } 

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Hide();
            Program.loginEstatico.Show();
        }

        private void Consultar_FormClosing(object sender, FormClosingEventArgs e) { instancia = null; Application.Exit(); }

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



        // Procedimineto que inicializa la información en el panel
        private void LlenarPanel()
        {
            // Proceso funcional para mostrar la información en el panel
            txtUsuario.Text = dgvRegistros.Rows[0].Cells[1].Value.ToString();
            txtEmail.Text = dgvRegistros.Rows[0].Cells[4].Value.ToString();
            txtTelefono.Text = dgvRegistros.Rows[0].Cells[5].Value.ToString();
            txtNombre.Text = dgvRegistros.Rows[0].Cells[6].Value.ToString();
            txtApellidoM.Text = dgvRegistros.Rows[0].Cells[7].Value.ToString();
            txtApellidoP.Text = dgvRegistros.Rows[0].Cells[8].Value.ToString();
            txtEdad.Text = dgvRegistros.Rows[0].Cells[9].Value.ToString();
            txtSexo.Text = dgvRegistros.Rows[0].Cells[10].Value.ToString();

            txtIndexPanel.Text = secuencia.ToString(); // Proceso visual para el textBox que sincroniza el índice
            indexMax = (int)TotalRegistros() - 1;     // Obtiene el limite superior para la navegación derecha
        }

        // Procedimiento que controla la navegación izquierda
        private void pbBack_Click(object sender, EventArgs e)
        {
            if (Index >= 1) // Evaluar límite inferior
            {
                // Proceso visual para el textBox que sincroniza el índice
                secuencia--; 
                txtIndexPanel.Text = secuencia.ToString();

                // Proceso funcional para mostrar la información en el panel
                Index--;
                txtUsuario.Text = dgvRegistros.Rows[Index].Cells[1].Value.ToString();
                txtEmail.Text = dgvRegistros.Rows[Index].Cells[4].Value.ToString();
                txtTelefono.Text = dgvRegistros.Rows[Index].Cells[5].Value.ToString();
                txtNombre.Text = dgvRegistros.Rows[Index].Cells[6].Value.ToString();
                txtApellidoM.Text = dgvRegistros.Rows[Index].Cells[7].Value.ToString();
                txtApellidoP.Text = dgvRegistros.Rows[Index].Cells[8].Value.ToString();
                txtEdad.Text = dgvRegistros.Rows[Index].Cells[9].Value.ToString();
                txtSexo.Text = dgvRegistros.Rows[Index].Cells[10].Value.ToString();

            }
            else // No corresponde
            { MessageBox.Show("No es posible realizar la operación, índice fuera de rango!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        // Procedimiento que controla la navegación derecha
        private void pbForward_Click(object sender, EventArgs e)
        {
            if (Index < indexMax) // Evaluar límite superior
            {
                // Proceso visual para el textBox que sincroniza el índice
                secuencia++; 
                txtIndexPanel.Text = secuencia.ToString();

                // Proceso funcional para mostrar la información en el panel
                Index++;
                txtUsuario.Text = dgvRegistros.Rows[Index].Cells[1].Value.ToString();
                txtEmail.Text = dgvRegistros.Rows[Index].Cells[4].Value.ToString();
                txtTelefono.Text = dgvRegistros.Rows[Index].Cells[5].Value.ToString();
                txtNombre.Text = dgvRegistros.Rows[Index].Cells[6].Value.ToString();
                txtApellidoM.Text = dgvRegistros.Rows[Index].Cells[7].Value.ToString();
                txtApellidoP.Text = dgvRegistros.Rows[Index].Cells[8].Value.ToString();
                txtEdad.Text = dgvRegistros.Rows[Index].Cells[9].Value.ToString();
                txtSexo.Text = dgvRegistros.Rows[Index].Cells[10].Value.ToString();
            }
            else // No corresponde
            { MessageBox.Show("No es posible realizar la operación, índice fuera de rango!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
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
                        { MessageBox.Show("Error " + ex.Message); }
                    }
                }
            }
            else
            { throw new Exception("Seleccione una fila para eliminar."); }
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


        // Función que intenta obtener la totalidad de registro
        private int? TotalRegistros()
        {
            // Emplear la conexión de la base de datos
            using (SqlConnection con = BD_Conexion.GetConnection())
            {
                // Definir la consulta
                query = "SELECT COUNT(*) FROM Usuarios;";
                Comando = new SqlCommand(query, con);

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


    }
}
