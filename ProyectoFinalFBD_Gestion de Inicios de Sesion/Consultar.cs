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
        SqlDataAdapter Adaptador = null;
        DataTable Tabla = new DataTable();
        string query = "", proceso = "Sin Búsqueda", usuarioNoModificado;
        int Index = 0, indexMax, secuencia = 1;
        int usuarioID = 0;


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
            Validar_EstadoInformacion();
        }



        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Hide();
            Program.loginEstatico.Show();
            secuencia = 1; Index = 0; LlenarPanel(); 
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
            txtContraseña.Text = dgvRegistros.Rows[0].Cells[2].Value.ToString();
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
                txtContraseña.Text = dgvRegistros.Rows[Index].Cells[2].Value.ToString();
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
                txtContraseña.Text = dgvRegistros.Rows[Index].Cells[2].Value.ToString();
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
            DialogResult confirmacion = MessageBox.Show("¿Está seguro que desea eliminar el registro?",
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.Yes)
            {
                switch (proceso)
                {
                    case "Búsqueda Exitosa": // ELIMINAR MEDIANTE LA BÚSQUEDA

                        // Recuperar ID del usuario para procesarlo
                        usuarioID = (int)ObtenerID(txtUsuario.Text);

                        // Emplear la conexión de la base de datos
                        using (SqlConnection con = BD_Conexion.GetConnection())
                        {
                            // Definir la consulta
                            query = "DELETE FROM Usuarios WHERE ID = @Id DELETE FROM IntentosDeSesion WHERE UsuarioID = @Id DELETE FROM EstadoDeLaSesion WHERE UsuarioID = @Id";
                            Comando = new SqlCommand(query, con);
                            Comando.Parameters.AddWithValue("@Id", usuarioID);

                            // Realizar la consulta
                            try
                            {
                                Comando.ExecuteNonQuery();
                                MessageBox.Show("Registro Eliminado");
                                ActualizarInfo(); // Reflejar en la tabla la eliminación
                                secuencia = 1; Index = 0;
                                Validar_EstadoInformacion();   // Reflejar en el panel la eliminación
                            }
                            catch (Exception ex)
                            { MessageBox.Show("Error " + ex.Message); }
                        }
                        break;

                    case "Búsqueda Fallida": // NO APLICA ELIMINAR MEDIANTE LA BÚSQUEDA(RESULTADO NO ENCONTRADO)

                        MessageBox.Show("No es posible realizar la operación. ¡No hay registro para eliminar!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    case "Sin Búsqueda": // ELIMINAR MEDIANTE EL PANEL

                        int ID = Convert.ToInt32(dgvRegistros.Rows[Index].Cells[0].Value.ToString()); // Guardar el indice 

                        // Emplear la conexión de la base de datos
                        using (SqlConnection con = BD_Conexion.GetConnection())
                        {
                            // Definir la consulta
                            query = "DELETE FROM Usuarios WHERE ID = @Id DELETE FROM IntentosDeSesion WHERE UsuarioID = @Id DELETE FROM EstadoDeLaSesion WHERE UsuarioID = @Id";
                            Comando = new SqlCommand(query, con);
                            Comando.Parameters.AddWithValue("@Id", ID);

                            // Realizar la consulta
                            try
                            {
                                Comando.ExecuteNonQuery();
                                MessageBox.Show("Registro Eliminado");
                                ActualizarInfo(); // Reflejar en la tabla la eliminación
                                secuencia = 1; Index = 0;
                                Validar_EstadoInformacion();   // Reflejar en el panel la eliminación
                            }
                            catch (Exception ex)
                            { MessageBox.Show("Error " + ex.Message); }
                        }
                        break;
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            switch (proceso)
            {
                case "Búsqueda Exitosa": // MODIFICAR MEDIANTE LA BÚSQUEDA

                    // Evaluar selección
                    if (txtUsuario.Enabled == false && txtUsuario.Enabled == false && txtEmail.Enabled == false && txtTelefono.Enabled == false && txtNombre.Enabled == false)
                    {
                        // EDICIÓN

                        usuarioNoModificado = txtUsuario.Text; // Guardar usuario antes de modificar
                        txtUsuario.Enabled = true;
                        txtContraseña.Enabled = true;
                        txtEmail.Enabled = true;
                        txtTelefono.Enabled = true;

                        btnModificar.Text = "CONFIRMAR";
                        btnModificar.ForeColor = Color.Red;

                        txtUsuario.Focus();
                    }
                    else
                    {
                        // CONFIRMACIÓN

                        if (ValidarCampoVacio()) // Comprobar
                        {
                            txtUsuario.Enabled = false;
                            txtContraseña.Enabled = false;
                            txtEmail.Enabled = false;
                            txtTelefono.Enabled = false;
                            btnModificar.Text = "MODIFICAR";
                            btnModificar.ForeColor = Color.FromArgb(40, 103, 206);

                            // Recuperar ID del usuario para procesarlo
                            usuarioID = (int)ObtenerID(usuarioNoModificado);

                            // Emplear la conexión de la base de datos
                            using (SqlConnection con = BD_Conexion.GetConnection())
                            {
                                // Definir la consulta
                                query = "UPDATE Usuarios SET Usuario = @Usuario, Contrasena = @Contrasena, CorreoElectronico = @CorreoElectronico, @Telefono = Telefono WHERE ID = @ID";
                                Comando = new SqlCommand(query, con);

                                Comando.Parameters.AddWithValue("@Usuario", txtUsuario.Text);
                                Comando.Parameters.AddWithValue("@Contrasena", txtContraseña.Text);
                                Comando.Parameters.AddWithValue("@CorreoElectronico", txtEmail.Text);
                                Comando.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                                Comando.Parameters.AddWithValue("@ID", usuarioID);

                                // Realizar la consulta
                                try
                                {
                                    Comando.ExecuteNonQuery();
                                    MessageBox.Show("Registro Actualizado!");

                                    // Reiniciar
                                    secuencia = 1; Index = 0;
                                    txtBuscar.Clear();
                                    txtIndexPanel.Visible = true;
                                    proceso = "Sin Búsqueda";
                                    ActualizarInfo();
                                    LlenarPanel();
                                }
                                catch (Exception ex) { MessageBox.Show("Error " + ex.Message); }
                            }
                        }
                        else { MessageBox.Show("Se requiere llenar todos los campos"); }
                    }
                    break;

                case "Búsqueda Fallida": // NO APLICA MODIFICAR MEDIANTE LA BÚSQUEDA(RESULTADO NO ENCONTRADO)

                    MessageBox.Show("No es posible realizar la operación. ¡No hay registro para modificar!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;

                case "Sin Búsqueda": // MODIFICAR MEDIANTE EL PANEL

                    // Evaluar selección
                    if (txtUsuario.Enabled == false && txtUsuario.Enabled == false && txtEmail.Enabled == false && txtTelefono.Enabled == false && txtNombre.Enabled == false)
                    {
                        // EDICIÓN

                        txtUsuario.Enabled = true;
                        txtContraseña.Enabled = true;
                        txtEmail.Enabled = true;
                        txtTelefono.Enabled = true;

                        btnModificar.Text = "CONFIRMAR";
                        btnModificar.ForeColor = Color.Red;

                        txtUsuario.Focus();
                    }
                    else
                    {
                        // CONFIRMACIÓN

                        if (ValidarCampoVacio()) // Comprobar
                        {
                            txtUsuario.Enabled = false;
                            txtContraseña.Enabled = false;
                            txtEmail.Enabled = false;
                            txtTelefono.Enabled = false;
                            btnModificar.Text = "MODIFICAR";
                            btnModificar.ForeColor = Color.FromArgb(40, 103, 206);

                            // Emplear la conexión de la base de datos
                            using (SqlConnection con = BD_Conexion.GetConnection())
                            {
                                // Definir la consulta
                                query = "UPDATE Usuarios SET Usuario = @Usuario, Contrasena = @Contrasena, CorreoElectronico = @CorreoElectronico, @Telefono = Telefono WHERE ID = @ID";
                                Comando = new SqlCommand(query, con);

                                Comando.Parameters.AddWithValue("@Usuario", txtUsuario.Text);
                                Comando.Parameters.AddWithValue("@Contrasena", txtContraseña.Text);
                                Comando.Parameters.AddWithValue("@CorreoElectronico", txtEmail.Text);
                                Comando.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                                Comando.Parameters.AddWithValue("@ID", dgvRegistros.Rows[Index].Cells[0].Value.ToString());

                                // Realizar la consulta
                                try
                                {
                                    Comando.ExecuteNonQuery();
                                    MessageBox.Show("Registro Actualizado!");
                                    ActualizarInfo();
                                }
                                catch (Exception ex) { MessageBox.Show("Error " + ex.Message); }
                            }
                        }
                        else { MessageBox.Show("Se requiere llenar todos los campos"); }
                    }
                    break;
            }
        }
        


        // Procedimiento que maneja la búsqueda al cambiar el texto
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {   // Verificar si el cuadro de texto no está vacío
            if (txtBuscar.Text != "")
            {
                string criterio = txtBuscar.Text.ToUpper();  // Obtener el texto de búsqueda en mayúsculas //

                txtIndexPanel.Visible = false;
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
                        if (cell.Value != null && cell.Value.ToString().ToUpper().IndexOf(criterio) >= 0)//
                        { 
                            row.Visible = true; // Si encuentra una coincidencia, hace visible la fila

                            // Asignar los valores de la primera fila visible a los TextBox
                            txtUsuario.Text = row.Cells["Usuario"].Value.ToString();
                            txtContraseña.Text = row.Cells["Contrasena"].Value.ToString();
                            txtEmail.Text = row.Cells["CorreoElectronico"].Value.ToString();
                            txtTelefono.Text = row.Cells["Telefono"].Value.ToString();
                            txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                            txtApellidoP.Text = row.Cells["ApellidoP"].Value.ToString();
                            txtApellidoM.Text = row.Cells["ApellidoM"].Value.ToString();
                            txtEdad.Text = row.Cells["Edad"].Value.ToString();
                            txtSexo.Text = row.Cells["Sexo"].Value.ToString();

                            break; 
                        }  
                        
                    }
                }
                proceso = "Búsqueda Exitosa";

            }  // Si el cuadro de búsqueda está vacío, actualizar la información
            else { ActualizarInfo(); LlenarPanel(); txtIndexPanel.Visible = true; proceso = "Sin Búsqueda"; }

            // Evalua si no hay filas para reflejarlo en los textbox
            if (!dgvRegistros.Rows.Cast<DataGridViewRow>().Any(row => row.Visible == true))
            {   // No se encontraron filas que coincidan con la busqueda
                txtUsuario.Clear();
                txtContraseña.Clear();
                txtEmail.Clear();
                txtTelefono.Clear();
                txtNombre.Clear();
                txtApellidoP.Clear();
                txtApellidoM.Clear();
                txtEdad.Clear();
                txtSexo.Clear();

                proceso = "Búsqueda Fallida";
            }
        }

       




        // Funciones adicionales
        private bool ValidarCampoVacio()
        { return txtUsuario.Text != "" && txtContraseña.Text != "" && txtEmail.Text != "" && txtTelefono.Text != ""; }
        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        { if (e.KeyChar == Convert.ToChar(Keys.Enter)) { txtContraseña.Focus(); } }
        private void txtContraseña_KeyPress(object sender, KeyPressEventArgs e)
        { if (e.KeyChar == Convert.ToChar(Keys.Enter)) { txtEmail.Focus(); } }
        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        { if (e.KeyChar == Convert.ToChar(Keys.Enter)) { txtTelefono.Focus(); } }
        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter)) { btnModificar.Focus(); }
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("Solo se admiten números", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true; return;
            }
        }
        private void Validar_EstadoInformacion()
        {
            // Comprobar si tiene datos por mostrar
            if (dgvRegistros.Rows.Count == 0) // VACÍO
            { MessageBox.Show("No hay datos por mostrar. ¡Intente registrar usuarios!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning); this.Hide(); Program.loginEstatico.Show(); }
            else                            // NO VACÍO
            { LlenarPanel(); }
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
