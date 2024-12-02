using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ProyectoFinalFBD_Gestion_de_Inicios_de_Sesion
{
    //Clase para la inactividad del usuario
    public static class DatosInactividad
    {
        static public int control; //Indicador

        //Metodo de tipo TimeSpan para obtener el tiempo de inactividad
        public static TimeSpan GetInputIdleTime()
        {
            var plii = new NativeMethods.LastInputInfo(); //La variable obtiene los datos de la estructura LastInputInfo
            plii.cbSize = (UInt32)Marshal.SizeOf(plii);

            if (NativeMethods.GetLastInputInfo(ref plii))
            {
                //Calcula el timepo de inactividad
                return TimeSpan.FromMilliseconds(Environment.TickCount - plii.dwTime);
            }
            else
            { throw new Win32Exception(Marshal.GetLastWin32Error()); }
        }

        //Metodo de tipo DateTimeOffset para obtener la ultima de actividad realizada por el usuario
        public static DateTimeOffset GetLastInputTime()
        {
            //Se resta el tiempo actual con el tiempo actividad
            return DateTimeOffset.Now.Subtract(GetInputIdleTime());
        }


        private static class NativeMethods
        {
            //Estructura que contiene la informacion sobre la inactidad
            public struct LastInputInfo
            {
                public UInt32 cbSize; //Tamaño de la estructura
                public UInt32 dwTime; //Recuento de tics cuando se recibio la ultima actividad
            }

            [DllImport("user32.dll")] //Uso de la API GetLastInputInfo de Win32
            public static extern bool GetLastInputInfo(ref LastInputInfo plii);
        }
    }
}
