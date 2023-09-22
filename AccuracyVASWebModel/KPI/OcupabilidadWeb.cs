using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyModel.KPI
{
    public class OcupabilidadRequestWeb
    {
        public string id_almacen { get; set; }
        public string? fecha_inicial { get; set; }
        public string? fecha_final { get; set; }
        public int? area { get; set; }
    }
    public class OcupabilidadBodyWeb
    {
        public string unidad { get; set; }
        public decimal total { get; set; }
        public decimal ocupado { get; set; }
        public string area { get; set; }
        public string fecha { get; set; }
        public int actual { get; set; }
    }
}
