using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfacesComunicacion;
using System.Threading;

namespace CapaFuncional
{
    public class Brazo
    {
        public SortedList<string, Articulacion> articulaciones { get; set; }

        private static InterfaceComunicacion Comunicacion;

        public InterfaceComunicacion tipoComunicacion
        {
            get { return Comunicacion; }

        }

        public Posicion posicionActual { get; set; }

        private Single cabeceo = (-30);

        public void SetCabeceo(Single valor)
        {
            cabeceo = valor;
        }

        public Single GetCabeceo
        {
            get { return cabeceo; }
        }

        private static Brazo instance;

        public int alturaH { get; set; }

        public int longBrazo { get; set; }

        public int longAntBrazo { get; set; }

        public int longMuñeca { get; set; }

        private Brazo() { }

        public static Brazo GetInstance(InterfaceComunicacion TipoComunicacion)
        {
            if (instance == null)
            {
                instance = new Brazo();
                instance.articulaciones = new SortedList<string, Articulacion>();
                instance.articulaciones.Add("Base", new Base(TipoComunicacion));
                instance.articulaciones.Add("Hombro", new Hombro(TipoComunicacion));
                instance.articulaciones.Add("Codo", new Codo(TipoComunicacion));
                instance.articulaciones.Add("Muñeca", new Muñeca(TipoComunicacion));
                Comunicacion = TipoComunicacion;
            }
            return instance;
        }

        public static void KillInstance()
        {
            instance = null;
        }

        public void CambiarConexion(InterfaceComunicacion nuevaComunicacion)
        {
            articulaciones.Clear();
            instance.articulaciones.Add("Base", new Base(nuevaComunicacion));
            instance.articulaciones.Add("Hombro", new Hombro(nuevaComunicacion));
            instance.articulaciones.Add("Codo", new Codo(nuevaComunicacion));
            instance.articulaciones.Add("Muñeca", new Muñeca(nuevaComunicacion));
            Comunicacion = nuevaComunicacion;
        }


        //usando los movimientos en conjunto de las articulaciones
        #region Métodos para mover al brazo en su conjunto


        /// <summary>
        /// Mueve las articulaciones para desplazarse a la posicion indicada del espacio
        /// </summary>
        /// <param name="position"> posicion en el espacio (x,y,z)</param>
        public void goTo(Posicion position)
        {
            MovementCalculator calc = new MovementCalculator();
            List<Posicion> posiciones = calc.MiddlePositions(position,this);
            

            foreach(Posicion pos in posiciones)
            {
                foreach (Articulacion art in pos.articulaciones.Values)
                {
                    art.Mover(art.posicion);
                }
                //pos.articulaciones["Base"].Mover(pos.articulaciones["Base"].posicion);
                //pos.articulaciones["Hombro"].Mover(pos.articulaciones["Hombro"].posicion);
                //pos.articulaciones["Codo"].Mover(pos.articulaciones["Codo"].posicion);
                //pos.articulaciones["Muñeca"].Mover(pos.articulaciones["Muñeca"].posicion);
            }
                     


        }

        /// <summary>
        /// LLeva al brazo a la posicion cero
        /// </summary>
        public void PosicionCero()
        {
            Posicion posicionCero = new Posicion();
            posicionCero.dimensionX = -15;
            posicionCero.dimensionY = -150;
            posicionCero.dimensionZ = 150;
            posicionActual = posicionCero;
            MovementCalculator calc = new MovementCalculator();
            calc.ZeroPosition(posicionCero, this);
            posicionCero.articulaciones["Base"].Mover(posicionCero.articulaciones["Base"].posicion);
            posicionCero.articulaciones["Hombro"].Mover(posicionCero.articulaciones["Hombro"].posicion);
            posicionCero.articulaciones["Codo"].Mover(posicionCero.articulaciones["Codo"].posicion);
            posicionCero.articulaciones["Muñeca"].Mover(posicionCero.articulaciones["Muñeca"].posicion);         
        }
        #endregion
    }

}
