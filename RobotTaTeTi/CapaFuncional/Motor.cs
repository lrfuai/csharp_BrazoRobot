using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CapaLogica;
using InterfacesComunicacion;

namespace CapaFuncional
{
    public class Motor
    {
        //atributo que permite que el método de girarGrados sea polimórfico
        //configurar antes de llamar a girarGrados
        InterfaceComunicacion Comunicacion;
        int Numero;

        public void EstablecerNumero(int numero)
        {
            Numero = numero;
        }

        public int DevolverNumero()
        {
            return Numero;
        }

        public void EstablecerComunicacion(InterfaceComunicacion com)
        {
            Comunicacion = com;
        }

        public void girarGrados(float grados)
        {
            //convierto los grados a posiciones que admite el servo
            byte posicion = (byte)(grados * 254 / 180);
            Comunicacion.moveTo(posicion, (byte)Numero);
        }

        //Mueve el motor sin necesidad de casteo de la posicion (cada articulacion sabe cual es su posicion 0)
        public void PositionZero(byte posicion)
        {
            Comunicacion.moveTo(posicion, (byte)Numero);
        }
    }
}
