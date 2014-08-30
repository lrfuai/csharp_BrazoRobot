using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaFuncional
{
    public class Posicion
    {
        public Single dimensionX { get; set; }
        public Single dimensionY { get; set; }
        public Single dimensionZ { get; set; }

        public SortedList<string, Articulacion> articulaciones { get; set; }

    }
}
