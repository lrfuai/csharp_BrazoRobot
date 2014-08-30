using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CapaFuncional
{
    public abstract class Articulacion
    {
        public Motor motor = new Motor();

        public abstract void PosicionarEnCero();

        public float posicion { get; set; }


        public virtual void Mover(float grados)
        {
            if (grados > 180)
            {
                grados = 180;
            }
            this.motor.girarGrados(grados);
            this.posicion = grados;
        }
    }
}
