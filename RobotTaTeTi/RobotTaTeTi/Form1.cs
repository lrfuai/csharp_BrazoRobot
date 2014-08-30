using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CapaFuncional;
using InterfacesComunicacion;
using System.Threading;

namespace RobotTaTeTi
{
    public partial class Form1 : Form
    {
        Brazo brazoPrincipal;
        public Form1()
        {
            InitializeComponent();
            brazoPrincipal = Brazo.GetInstance(Conexiones.RetornarConexion(tipoConexion.ConexionSerie));
            brazoPrincipal.longAntBrazo = 95;
            brazoPrincipal.longBrazo = 90;
            brazoPrincipal.longMuñeca = 45;
            brazoPrincipal.alturaH = 85;
            brazoPrincipal.PosicionCero();

        }

        private void button1_Click(object sender, EventArgs e)
        {          
            Posicion position = new Posicion();
            position.dimensionX = Single.Parse(txtX.Text);
            position.dimensionY = Single.Parse(txtY.Text);
            position.dimensionZ = Single.Parse(txtZ.Text);
            brazoPrincipal.goTo(position);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            brazoPrincipal.SetCabeceo(float.Parse(textBox1.Text));
        }

    }
}
