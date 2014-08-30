using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfacesComunicacion;

namespace CapaFuncional
{
    class Codo : Articulacion
    {
      
        public Codo(InterfaceComunicacion queComunicacion)
        {
            this.motor = new Motor();
            motor.EstablecerNumero(3);
            motor.EstablecerComunicacion(queComunicacion);
        }
        #region Miembros de Articulacion

        public override void Mover(float grados)
        {
            //se trata del servo que se mueve al revés que los demás
            //invierto los grados para obtener el movimiento deseado
            this.motor.girarGrados(180 - grados);
            this.posicion = grados;
        }

        public override void PosicionarEnCero()
        {
            this.motor.PositionZero(135);
            this.posicion = 47;
        }

        #endregion
    }
}
