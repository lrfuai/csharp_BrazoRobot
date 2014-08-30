using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfacesComunicacion;

namespace CapaFuncional
{
    class Hombro : Articulacion
    {
       
        public Hombro(InterfaceComunicacion queComunicacion)
        {
            this.motor = new Motor();
            motor.EstablecerNumero(2);
            motor.EstablecerComunicacion(queComunicacion);
        }
        #region Miembros de Articulacion


        public override void PosicionarEnCero()
        {
            this.motor.PositionZero(170);
            this.posicion = 60;
        }
        #endregion
    }
}
