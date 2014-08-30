using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace InterfacesComunicacion
{
    public interface InterfaceComunicacion
    {
        void moveTo(byte position, byte articulacion);
        void closeConnection();
        void OpenConnection();
    }
    public enum tipoConexion
    {
        ConexionSerie,
        Bluetooth

    }




}
