using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace ProyectoFinalFBD_Gestion_de_Inicios_de_Sesion
{
    internal class BD_Conexion
    {
        private static string Conexion = "Data Source =¿?; Initial Catalog = GestionInicioSesion; integrated security=true";

        public static SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection(Conexion);

            //Valida el estado de la conexion
            if (con.State != System.Data.ConnectionState.Open)
            { con.Open(); }

            return con;
        }
    }
}
