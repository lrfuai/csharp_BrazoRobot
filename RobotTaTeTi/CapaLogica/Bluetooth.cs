using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using InterfacesComunicacion;

namespace CapaLogica
{
    public class Bluetooth: InterfaceComunicacion
    {

        #region Miembros de InterfaceComunicacion

        public void moveTo(byte position, byte articulacion)
        {
            throw new NotImplementedException();
        }

        public void closeConnection()
        {
            throw new NotImplementedException();
        }

        public void OpenConnection()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
