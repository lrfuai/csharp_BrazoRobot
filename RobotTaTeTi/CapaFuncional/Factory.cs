using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using InterfacesComunicacion;
using CapaLogica;

namespace CapaFuncional
{
    public class Conexiones
    {
        public static InterfaceComunicacion RetornarConexion(tipoConexion tipo)
        {
            return FactoryDeComunicaciones.RetornarInstancia(tipo);
        }
    }

   

}
