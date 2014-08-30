using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using InterfacesComunicacion;

namespace CapaLogica
{
    public class ConexionSerie : InterfaceComunicacion
    {
        SerialPort serial;

        //Primer byte siempre 255 de Inicialización
        byte[] comando = { 255, 0, 0 };

        public void closeConnection()
        {
            serial.Close();
        }

        public ConexionSerie(string PuertoCom,int baudRate, int dataBits)
        {
            serial = new SerialPort(PuertoCom);
            serial.BaudRate = baudRate;
            serial.DataBits = dataBits;
        }

        public ConexionSerie()
        {
            serial = new SerialPort("COM1");
            serial.BaudRate = 9600;
            serial.DataBits = 8;
        }


        public void OpenConnection()
        {
            serial.Open();
        }
        #region Miembros de InterfaceComunicacion

        public void moveTo(byte position, byte articulacion)
        {
            //Abro el puerto serie
            this.OpenConnection();
            //Segundo byte es el servomotor
            comando[1] = articulacion;
            //Tercer byte es la posicion entre 0 y 254.
            comando[2] = position;
            serial.Write(comando, 0, 3);
            //Cierro el puerto siempre.
            this.closeConnection();
        }

        #endregion
    }
}
