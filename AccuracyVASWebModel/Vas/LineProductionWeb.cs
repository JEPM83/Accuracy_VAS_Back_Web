using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyModel.Vas
{
    public class LineRequest
    {
        public string id_almacen { get; set; }
        public string direccion { get; set; }
    }
    public class LineResponse
    {
        public string linea_produccion { get; set; }
        public int estado { get; set; }
        public string mensaje { get; set; }
    }
}