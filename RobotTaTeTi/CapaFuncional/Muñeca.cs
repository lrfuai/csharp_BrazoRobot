using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfacesComunicacion;

namespace CapaFuncional
{
    class Muñeca: Articulacion
    {
         
        public Muñeca(InterfaceComunicacion queComunicacion)
        {
            this.motor = new Motor();
            motor.EstablecerNumero(4);
            motor.EstablecerComunicacion(queComunicacion);
        }
        #region Miembros de Articulacion

        public override void PosicionarEnCero()
        {
            this.motor.PositionZero(230);
            this.posicion = 81;
        }

        #endregion
    }
}
