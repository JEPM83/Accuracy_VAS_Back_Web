using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AccuracyModel.General
{
    public class GeneralObjetoRequestWeb
    {
        public string objeto { get; set; }
        public string tipo { get; set; }
        public string idioma { get; set; }
        public bool modo { get; set; }
    }
    public class GeneralObjetotBodyWeb
    {
        public string value { get; set; }
        public string display { get; set; }
        public string display_large { get; set; }
    }
    /*Envio de curvas - solo ITSANET*/
    public class CurvaRequest
    {
        public string id_almacen { get; set; }
        public string numero_orden_compra { get; set; }
    }
    public class CurvaDetalle
    {
        [JsonPropertyName("EAN")]
        public string EAN { get; set; }
        [JsonPropertyName("CANTIDAD")]
        public int CANTIDAD { get; set; }
        [JsonPropertyName("ERROR")]
        public string ERROR { get; set; }
        [JsonPropertyName("MESSAGE")]
        public object MESSAGE { get; set; }
    }
    public class CurvaRoot
    {
        [JsonPropertyName("COD. EMPRESA")]
        public string COD_EMPRESA { get; set; }
        [JsonPropertyName("NRO. PACKING")]
        public string NRO_PACKING { get; set; }
        [JsonPropertyName("CANTIDAD BULTOS")]
        public int CANTIDAD_BULTOS { get; set; }
        [JsonPropertyName("ORDEN DE COMPRA")]
        public string ORDEN_DE_COMPRA { get; set; }
        [JsonPropertyName("ERROR")]
        public string ERROR { get; set; }
        [JsonPropertyName("DETALLE")]
        public List<CurvaDetalle> DETALLE { get; set; }
    }
}
