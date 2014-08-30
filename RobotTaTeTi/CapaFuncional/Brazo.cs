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
        SortedList<string, Articulacion> articulaciones;

        private Posicion posicionActual = new Posicion();

        public void EstablecerPosicion(Posicion position)
        {
            posicionActual.dimensionX = position.dimensionX;
            posicionActual.dimensionY = position.dimensionY;
            posicionActual.dimensionZ = position.dimensionZ;
        }

        private double AngGiro, AngBrazo, AngAntBr, AngMuñecaA;

        private Single balance = 0;
        private Single cabeceo = (-30);

        Single Xaux = 0;
        Single Yaux = 0;

        Single Zaux = 0;
        Single Baux = 0;
        Single Caux = 0;


        private const double Grados = 180 / Math.PI;

        private const double Rad = Math.PI / 180;

        public void SetCabeceo(Single valor)
        {
            cabeceo = valor;
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
        }


        //usando los movimientos en conjunto de las articulaciones
        #region Métodos para mover al brazo en su conjunto


        public void InverseK(Posicion position)
        {
            double Afx, Afy, LadoA, LadoB, alfa, beta, gamma, modulo, hipotenusa, xprima, yprima;

            //calcula angulo de la base
            AngGiro = Math.Atan2(position.dimensionY, position.dimensionX) * Grados;

            modulo = Math.Sqrt(Math.Abs(Math.Pow(position.dimensionX, 2)) + Math.Abs(Math.Pow(position.dimensionY, 2)));
            xprima = modulo;
            yprima = position.dimensionZ;
            //Afx = Math.Cos(Rad*articulaciones["Muñeca"].posicion)*(this.longMuñeca + 140);
            Afx = Math.Cos(Rad * (cabeceo)) * (this.longMuñeca + 95);
            LadoB = xprima - Afx;
            //Afy = Math.Sin(Rad * articulaciones["Muñeca"].posicion) * (this.longMuñeca + 140);
            Afy = Math.Sin(Rad * (cabeceo)) * (this.longMuñeca + 95);
            LadoA = yprima - Afy - alturaH;
            hipotenusa = Math.Sqrt((LadoA * LadoA) + (LadoB * LadoB));
            alfa = Math.Atan2(LadoA, LadoB);
            beta = Math.Acos(((Math.Pow(longBrazo, 2)) - (Math.Pow(longAntBrazo, 2)) + (Math.Pow(hipotenusa, 2))) / (2 * longBrazo * hipotenusa));

            //calcula el angulo del hombro
            AngBrazo = (alfa + beta) * Grados;
            gamma = Math.Acos(((Math.Pow(longBrazo, 2)) + (Math.Pow(longAntBrazo, 2)) - (Math.Pow(hipotenusa, 2))) / (2 * longBrazo * longAntBrazo));
            //calcula el angulo del codo
            AngAntBr = (-((180 * Rad) - gamma)) * Grados;


            AngMuñecaA = cabeceo - AngBrazo - AngAntBr;




            if (double.IsNaN(AngBrazo) || double.IsNaN(AngAntBr))
            {
                position.dimensionX = Xaux;
                position.dimensionY = Yaux;
                position.dimensionZ = Zaux;
                balance = Baux;
                cabeceo = Caux;
                InverseK(position);
            }

            Xaux = position.dimensionX;
            Yaux = position.dimensionY;
            Zaux = position.dimensionZ;
            Baux = balance;
            Caux = cabeceo;









            articulaciones["Base"].Mover((float)Math.Abs(AngGiro));
            articulaciones["Hombro"].Mover((float)Math.Abs(AngBrazo));
            articulaciones["Codo"].Mover((float)Math.Abs(AngAntBr + 180));
            articulaciones["Muñeca"].Mover((float)Math.Abs(AngMuñecaA + 90));

            //articulaciones["Base"].Mover((float)Math.Abs(AngGiro));
            //articulaciones["Hombro"].Mover((float)Math.Abs(AngBrazo));
            //articulaciones["Codo"].Mover((float)Math.Abs(AngAntBr));
            //articulaciones["Muñeca"].Mover((float)Math.Abs(AngMuñecaA));






        }

        //mueve las articulaciones para desplazarse a la posicion indicada
        public void goTo(Posicion position)
        {
            int cont, dx, dy, dz, Adx, Ady, Adz, x_inc, y_inc, z_inc, err_1, err_2, dx2, dy2, dz2, xxx, yyy, zzz, Xold, Yold, Zold, Xnew, Ynew, Znew;
            int incremento = 1;
            Xold = (int)posicionActual.dimensionX;
            Yold = (int)posicionActual.dimensionY;
            Zold = (int)posicionActual.dimensionZ;
            Xnew = (int)position.dimensionX;
            Ynew = (int)position.dimensionY;
            Znew = (int)position.dimensionZ;
            xxx = (int)posicionActual.dimensionX;
            yyy = (int)posicionActual.dimensionY;
            zzz = (int)posicionActual.dimensionZ;
            dx = Xnew - Xold;
            dy = Ynew - Yold;
            dz = Znew - Zold;
            if (dx < 0)
            {
                x_inc = -1;
            }
            else
            {
                x_inc = 1;
            }
            if (dy < 0)
            {
                y_inc = -1;
            }
            else
            {
                y_inc = 1;
            }
            if (dz < 0)
            {
                z_inc = -1;
            }
            else
            {
                z_inc = 1;
            }

            Adx = Math.Abs(dx);
            Ady = Math.Abs(dy);
            Adz = Math.Abs(dz);

            dx2 = Adx * 2;
            dy2 = Ady * 2;
            dz2 = Adz * 2;


            if ((Adx >= Ady) && (Adx >= Adz))
            {
                err_1 = dy2 - Adx;
                err_2 = dz2 - Adx;
                if (Adx > 10)
                {
                    //x_inc = 5*x_inc;
                    incremento = 3;
                }
                for (cont = 0; cont <= Adx - 1; cont+=incremento)
                {
                    if (err_1 > 0)
                    {
                        yyy += y_inc;
                        err_1 -= dx2;
                    }
                    if (err_2 > 0)
                    {
                        zzz += z_inc;
                        err_2 -= dx2;
                    }
                    err_1 += dy2;
                    err_2 += dz2;
                    xxx += x_inc*incremento;
                    Posicion posicionIntermedia = new Posicion();
                    posicionIntermedia.dimensionX = xxx;
                    posicionIntermedia.dimensionY = yyy;
                    posicionIntermedia.dimensionZ = zzz;
                    InverseK(posicionIntermedia);
                
                    if (Cercania(Adx - Math.Abs(cont)))
                    {
                        incremento = 1;
                        //if (dx < 0)
                        //{
                        //    x_inc = -1;
                        //}
                        //else
                        //{
                        //    x_inc = 1;
                        //}
                    }
                }

            }

            if ((Ady > Adx) && (Ady >= Adz))
            {
                err_1 = dx2 - Ady;
                err_2 = dz2 - Ady;
                if (Ady > 10)
                {
                    //y_inc = 5*y_inc;
                    incremento = 3;
                }
                for (cont = 0; cont <= Ady - 1; cont+= incremento)
                {
                    if (err_1 > 0)
                    {
                        xxx += x_inc;
                        err_1 -= dy2;
                    }
                    if (err_2 > 0)
                    {
                        zzz += z_inc;
                        err_2 -= dy2;
                    }
                    err_1 += dx2;
                    err_2 += dz2;
                    yyy += y_inc*incremento;
                    Posicion posicionIntermedia = new Posicion();
                    posicionIntermedia.dimensionX = xxx;
                    posicionIntermedia.dimensionY = yyy;
                    posicionIntermedia.dimensionZ = zzz;
                    InverseK(posicionIntermedia);
                    
                    if (Cercania(Ady - Math.Abs(cont)))
                    {
                        incremento = 1;
                        //if (dy < 0)
                        //{
                        //    y_inc = -1;
                        //}
                        //else
                        //{
                        //    y_inc = 1;
                        //}
                    }
                }

            }

            if ((Adz > Adx) && (Adz > Ady))
            {
                err_1 = dy2 - Adz;
                err_2 = dx2 - Adz;
                if (Adz > 10)
                {
                    //z_inc = 5*z_inc;
                    incremento = 3;
                }
                for (cont = 0; cont <= Adz - 1; cont+=incremento)
                {
                    if (err_1 > 0)
                    {
                        yyy += y_inc;
                        err_1 -= dz2;
                    }
                    if (err_2 > 0)
                    {
                        xxx += x_inc;
                        err_2 -= dy2;
                    }
                    err_1 += dy2;
                    err_2 += dx2;
                    zzz += z_inc*incremento;
                    Posicion posicionIntermedia = new Posicion();
                    posicionIntermedia.dimensionX = xxx;
                    posicionIntermedia.dimensionY = yyy;
                    posicionIntermedia.dimensionZ = zzz;
                    InverseK(posicionIntermedia);
                   
                    if (Cercania(Adz - Math.Abs(cont)))
                    {
                        incremento = 1;
                        //if (dz < 0)
                        //{
                        //    z_inc = -1;
                        //}
                        //else
                        //{
                        //    z_inc = 1;
                        //}
                    }
                }

            }

            posicionActual.dimensionX = Xnew;
            posicionActual.dimensionY = Ynew;
            posicionActual.dimensionZ = Znew;


        }

        public bool Cercania(int posicion)
        {
            if (posicion <= 10)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void PosicionCero()
        {
            Posicion posicionCero = new Posicion();
            posicionCero.dimensionX = -15;
            posicionCero.dimensionY = -150;
            posicionCero.dimensionZ = 150;
            EstablecerPosicion(posicionCero);
            InverseK(posicionCero);
            //foreach (Articulacion art in articulaciones.Values)
            //{
            //    art.PosicionarEnCero();
            //}
        }
        #endregion
    }

    public class Posicion
    {
        public Single dimensionX { get; set; }
        public Single dimensionY { get; set; }
        public Single dimensionZ { get; set; }
    }



}
