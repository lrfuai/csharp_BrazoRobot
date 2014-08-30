using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfacesComunicacion;

namespace CapaFuncional
{
    class Base: Articulacion
    {
        
        public Base(InterfaceComunicacion queComunicacion)
        {
            this.motor = new Motor();
            motor.EstablecerNumero(1);
            motor.EstablecerComunicacion(queComunicacion);
        }
        #region Miembros de Articulacion

        public override void PosicionarEnCero()
        {
            this.motor.PositionZero(150);
            this.posicion = 53;
        }

        #endregion
    }
}
