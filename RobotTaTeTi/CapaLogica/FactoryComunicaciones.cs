using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfacesComunicacion;
using System.Reflection;

namespace CapaLogica
{
    public class FactoryDeComunicaciones
    {

        public static InterfaceComunicacion RetornarInstancia(tipoConexion tipo)
        {
            return (InterfaceComunicacion)Activator.CreateInstanceFrom(Assembly.GetExecutingAssembly().Location, "CapaLogica." + tipo).Unwrap();
        }

    }
}
