using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyModel.Forecast
{
    public class ForecastHeadRequestWeb
    {
        public string id_almacen { get; set; }
    }
    public class ForecasHeadtBodyWeb
    {
        public int value { get; set; }
        public string display { get; set; }
    }
    #region Detail Forecast
    public class ForecastDetailRequestWeb
    {
        public string id_almacen { get; set; }
        public string? forecast { get; set; }
    }
    public class ForecastDetailBodyWeb
    {
        public string alerta { get; set; }
        public string? forecast { get; set; }
        public string? numero_item { get; set; }
        public string descripcion_item { get; set; }
        public string categoria_inventario { get; set; }
        public string? subcategoria_inventario { get; set; }
        public string? atributo_01 { get; set; }
        public float? cantidad { get; set; }
        public float? cantidad_recibir { get; set; }
        public string? usuario_creacion { get; set; }
        public string? fecha_creacion { get; set; }
        public string? usuario_modifica { get; set; }
        public string? fecha_modifica { get; set; }
        public int id { get; set; }
    }
    public class ForecastDetailUpdateRequestWeb
    {
        public string id_almacen { get; set; }
        public string? forecast { get; set; }
        public string? id_d_forecast { get; set; }
        public float? cantidad { get; set; }
        public float? cantidad_recibir { get; set; }
        public string? usuario { get; set; }
    }
    public class ForecastDetailUpdateBodyWeb {
        public int tipo { get; set; }
        public string mensaje { get; set; }
        public List<ForecastDetailUpdateDataWeb> data { get; set; }
    }
    public class ForecastDetailUpdateDataWeb
    {
        public string alerta { get; set; }
        public string? forecast { get; set; }
        public string? numero_item { get; set; }
        public string descripcion_item { get; set; }
        public string categoria_inventario { get; set; }
        public string? subcategoria_inventario { get; set; }
        public string? atributo_01 { get; set; }
        public float? cantidad { get; set; }
        public float? cantidad_recibir { get; set; }
        public string? usuario_creacion { get; set; }
        public string? fecha_creacion { get; set; }
        public string? usuario_modifica { get; set; }
        public string? fecha_modifica { get; set; }
        public int id { get; set; }
    }

    #endregion
}
