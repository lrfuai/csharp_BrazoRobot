using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CapaFuncional
{
    class MovementCalculator
    {
        private double AngGiro, AngBrazo, AngAntBr, AngMuñecaA;

        private Single balance = 0;
        //private Single cabeceo = (-30);

        Single Xaux = 0;
        Single Yaux = 0;
        Single Zaux = 0;
        Single Baux = 0;
        Single Caux = 0;

        private const double Grados = 180 / Math.PI;

        private const double Rad = Math.PI / 180;


        private void InverseK(Posicion position, Brazo brazo)
        {
            double Afx, Afy, LadoA, LadoB, alfa, beta, gamma, modulo, hipotenusa, xprima, yprima;

            //calcula angulo de la base
            AngGiro = Math.Atan2(position.dimensionY, position.dimensionX) * Grados;

            modulo = Math.Sqrt(Math.Abs(Math.Pow(position.dimensionX, 2)) + Math.Abs(Math.Pow(position.dimensionY, 2)));
            xprima = modulo;
            yprima = position.dimensionZ;

            Afx = Math.Cos(Rad * (brazo.GetCabeceo)) * (brazo.longMuñeca + 95);
            LadoB = xprima - Afx;

            Afy = Math.Sin(Rad * (brazo.GetCabeceo)) * (brazo.longMuñeca + 95);
            LadoA = yprima - Afy - brazo.alturaH;
            hipotenusa = Math.Sqrt((LadoA * LadoA) + (LadoB * LadoB));
            alfa = Math.Atan2(LadoA, LadoB);
            beta = Math.Acos(((Math.Pow(brazo.longBrazo, 2)) - (Math.Pow(brazo.longAntBrazo, 2)) + (Math.Pow(hipotenusa, 2))) / (2 * brazo.longBrazo * hipotenusa));

            //calcula el angulo del hombro
            AngBrazo = (alfa + beta) * Grados;
            gamma = Math.Acos(((Math.Pow(brazo.longBrazo, 2)) + (Math.Pow(brazo.longAntBrazo, 2)) - (Math.Pow(hipotenusa, 2))) / (2 * brazo.longBrazo * brazo.longAntBrazo));
            //calcula el angulo del codo
            AngAntBr = (-((180 * Rad) - gamma)) * Grados;


            AngMuñecaA = brazo.GetCabeceo - AngBrazo - AngAntBr;




            if (double.IsNaN(AngBrazo) || double.IsNaN(AngAntBr))
            {
                position.dimensionX = Xaux;
                position.dimensionY = Yaux;
                position.dimensionZ = Zaux;
                balance = Baux;
                brazo.SetCabeceo(Caux);
                InverseK(position,brazo);
            }

            Xaux = position.dimensionX;
            Yaux = position.dimensionY;
            Zaux = position.dimensionZ;
            Baux = balance;
            Caux = brazo.GetCabeceo;


            
            SortedList<string, Articulacion> articulaciones = new SortedList<string, Articulacion>();
            articulaciones.Add("Base",new Base(brazo.tipoComunicacion));
            articulaciones["Base"].posicion = (float)Math.Abs(AngGiro);
            articulaciones.Add("Hombro", new Hombro(brazo.tipoComunicacion));
            articulaciones["Hombro"].posicion = (float)Math.Abs(AngBrazo);
            articulaciones.Add("Codo", new Codo(brazo.tipoComunicacion));
            articulaciones["Codo"].posicion = (float)Math.Abs(AngAntBr + 180);
            articulaciones.Add("Muñeca", new Muñeca(brazo.tipoComunicacion));
            articulaciones["Muñeca"].posicion = (float)Math.Abs(AngMuñecaA + 90);
            position.articulaciones = articulaciones;
                    
            
        }

        public Posicion ZeroPosition(Posicion position,Brazo brazo)
        {
            InverseK(position, brazo);
            return position;
        }

        public List<Posicion> MiddlePositions(Posicion position, Brazo brazo)
        {
            List<Posicion> positions = new List<Posicion>();

            int cont, dx, dy, dz, Adx, Ady, Adz, x_inc, y_inc, z_inc, err_1, err_2, dx2, dy2, dz2, xxx, yyy, zzz, Xold, Yold, Zold, Xnew, Ynew, Znew;
            Xold = (int)brazo.posicionActual.dimensionX;
            Yold = (int)brazo.posicionActual.dimensionY;
            Zold = (int)brazo.posicionActual.dimensionZ;
            Xnew = (int)position.dimensionX;
            Ynew = (int)position.dimensionY;
            Znew = (int)position.dimensionZ;
            xxx = (int)brazo.posicionActual.dimensionX;
            yyy = (int)brazo.posicionActual.dimensionY;
            zzz = (int)brazo.posicionActual.dimensionZ;
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
                for (cont = 0; cont <= Adx - 1; cont++)
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
                    xxx += x_inc;
                    Posicion posicionIntermedia = new Posicion();
                    posicionIntermedia.dimensionX = xxx;
                    posicionIntermedia.dimensionY = yyy;
                    posicionIntermedia.dimensionZ = zzz;
                    InverseK(posicionIntermedia,brazo);
                    positions.Add(posicionIntermedia);
                }

            }

            if ((Ady > Adx) && (Ady >= Adz))
            {
                err_1 = dx2 - Ady;
                err_2 = dz2 - Ady;
                for (cont = 0; cont <= Ady - 1; cont++)
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
                    yyy += y_inc;
                    Posicion posicionIntermedia = new Posicion();
                    posicionIntermedia.dimensionX = xxx;
                    posicionIntermedia.dimensionY = yyy;
                    posicionIntermedia.dimensionZ = zzz;
                    InverseK(posicionIntermedia, brazo);
                    positions.Add(posicionIntermedia);
                }

            }

            if ((Adz > Adx) && (Adz > Ady))
            {
                err_1 = dy2 - Adz;
                err_2 = dx2 - Adz;
                for (cont = 0; cont <= Adz - 1; cont++)
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
                    zzz += z_inc;
                    Posicion posicionIntermedia = new Posicion();
                    posicionIntermedia.dimensionX = xxx;
                    posicionIntermedia.dimensionY = yyy;
                    posicionIntermedia.dimensionZ = zzz;
                    InverseK(posicionIntermedia, brazo);
                    positions.Add(posicionIntermedia);
                }

            }

            brazo.posicionActual.dimensionX = Xnew;
            brazo.posicionActual.dimensionY = Ynew;
            brazo.posicionActual.dimensionZ = Znew;


            float lastBase, lastHombro, lastCodo, lastMuñeca = 0;
            //brazo.articulaciones["Base"].

            if (positions.Count > 0)
            {
                lastBase = positions[0].articulaciones["Base"].posicion;
                lastCodo = positions[0].articulaciones["Codo"].posicion;
                lastHombro = positions[0].articulaciones["Hombro"].posicion;
                lastMuñeca = positions[0].articulaciones["Muñeca"].posicion;

                for (int i = 1; i < positions.Count - 1; i++)
                {

                    if (Math.Abs(positions[i].articulaciones["Base"].posicion - lastBase) < 0.7)
                        positions[i].articulaciones.Remove("Base");
                    else
                        lastBase = positions[i].articulaciones["Base"].posicion;

                    if (Math.Abs(positions[i].articulaciones["Hombro"].posicion - lastHombro) < 0.7)
                        positions[i].articulaciones.Remove("Hombro");
                    else
                        lastHombro = positions[i].articulaciones["Hombro"].posicion;

                    if (Math.Abs(positions[i].articulaciones["Codo"].posicion - lastCodo) < 0.7)
                        positions[i].articulaciones.Remove("Codo");
                    else
                        lastCodo = positions[i].articulaciones["Codo"].posicion;

                    if (Math.Abs(positions[i].articulaciones["Muñeca"].posicion - lastMuñeca) < 0.7)
                        positions[i].articulaciones.Remove("Muñeca");
                    else
                        lastMuñeca = positions[i].articulaciones["Muñeca"].posicion;
                }

            }
            return positions;
        
        }
    }
}
