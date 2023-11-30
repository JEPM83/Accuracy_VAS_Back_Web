using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyModel.Vas
{
    public class SendB2BVas_baseRequest {
        public string id_almacen { get; set; }
        public string numero_pedido { get; set; }
        public string? fecha_inicial_despacho { get; set; }
        public string? fecha_final_despacho { get; set; }
        public int? id_pedido { get; set; }
        public string? id_destino { get; set; }
        public string? cita { get; set; }
        public string? fecha_factura { get; set; }
        public string? factura { get; set; }
        public string? lote { get; set; }
    }
    public class SendB2BVas_atributos{
        public int columna { get; set; }
        public int fila { get; set; }
        public int? salto { get; set; }
        public string? titulo { get; set; }
        public string nombre_archivo { get; set; }
        public string extension_archivo { get; set; }
        public string hoja { get; set; }
        public string color_fondo_titulo_grilla { get; set; }
        public string color_letra_titulo_grilla { get; set; }
    }
    public class SendB2BVas_Modelo1Detalle : SendB2BVas_atributos //Ripley
    { 
        public string? numero_de_cita { get; set; }
        public string numero_de_oc { get; set; }
        public string ruc { get; set; }
        public string fecha { get; set; }
        public string documento { get; set; }
        public string contenedor { get; set; }

        public int correlativo { get; set; }
        public string articulo { get; set; }
        public float cantidad { get; set; }
        public string na { get; set; }
        public string cod_sucursal { get; set; }
        public float? costo_unitario { get; set; }
    }
    public class SendB2BVas_Modelo2Detalle : SendB2BVas_atributos//Saga
    { 
        public string nro_lote { get; set; }

        public string negocio { get; set; }
        public string orden_de_compra { get; set; }
        public string upc_ean { get; set; }
        public string descripcion { get; set; }
        public string caducidad { get; set; }
        public float cantidad { get; set; }
        public float cantidad_a_enviar { get; set; }
        public string tienda_destino { get; set; }
        public string nombre_tienda { get; set; }
        public string tipo_embalaje { get; set; }
        public string lpn { get; set; }
    }
    public class SendB2BVas_Modelo3Detalle : SendB2BVas_atributos //Tiendas Peruanas
    {
        public string codigo_proveedor { get; set; }
        public string sucursal_destino { get; set; }
        public string n_de_oc { get; set; }
        public string lpn { get; set; }
        public string codigo_producto { get; set; }
        public float cantidad_und { get; set; }
    }
    public class SendB2BVas_Modelo4Detalle : SendB2BVas_atributos //Otros
    {
        public string o_r { get; set; }
        public string o_c { get; set; }
        public string id_destino { get; set; }
        public string nombre_destino { get; set; }
        public string destino_cod { get; set; }
        public string destino_des { get; set; }
        public string sku { get; set; }
        public string item { get; set; }
        public string talla { get; set; }
        public float? cantidad { get; set; }
        public string ean13 { get; set; }
        public string precio { get; set; }
        public string lpn { get; set; }
    }
    public class RootB2BResponse
    {
        [JsonPropertyName("cabecera")]
        public CabeceraB2BResponse? cabeceraB2BResponse { get; set; }
        [JsonPropertyName("cuerpo")]
        public List<CuerpoB2BResponse> cuerpoB2BResponse { get; set; }
        [JsonPropertyName("pie")]
        public PieB2BResponse pieB2BResponse { get; set; }
    }
    public class CabeceraB2BResponse {
        [JsonPropertyName("Numero de Cita")]
        public string? numero_de_cita { get; set; }
        [JsonPropertyName("Numero de O/C")]
        public string? numero_de_oc { get; set; }
        [JsonPropertyName("RUC")]
        public string? ruc { get; set; }
        [JsonPropertyName("Fecha")]
        public string? fecha { get; set; }
        [JsonPropertyName("Documento")]
        public string? documento { get; set; }
        [JsonPropertyName("Contenedor")]
        public string? contenedor { get; set; }
        [JsonPropertyName("NRO. LOTE")]
        public string? nro_lote { get; set; }
    }
    public class PieB2BResponse : SendB2BVas_atributos
    {

    }
    public class CuerpoB2BResponse
    {
        [JsonPropertyName("Correlativo")]
        public int? correlativo { get; set; }
        [JsonPropertyName("Articulo")]
        public string? articulo { get; set; }
        [JsonPropertyName("Cantidad")]
        public float? cantidad { get; set; }
        [JsonPropertyName("NA")]
        public string? na { get; set; }
        [JsonPropertyName("Cod.Sucursal")]
        public string? cod_sucursal { get; set; }
        [JsonPropertyName("Costo unitario")]
        public float? costo_unitario { get; set; }
        [JsonPropertyName("NEGOCIO")]
        public string? negocio { get; set; }
        [JsonPropertyName("ORDEN DE COMPRA")]
        public string? orden_de_compra { get; set; }
        [JsonPropertyName("UPC/EAN")]
        public string? upc_ean { get; set; }
        [JsonPropertyName("SKU")]
        public string? sku { get; set; }
        [JsonPropertyName("DESCRIPCION")]
        public string? descripcion { get; set; }
        [JsonPropertyName("CADUCIDAD")]
        public string? caducidad { get; set; }
        //[JsonPropertyName("CANTIDAD_1")]
        //public float? cantidad_1 { get; set; }
        [JsonPropertyName("CANTIDAD A ENVIAR")]
        public float? cantidad_a_enviar { get; set; }
        [JsonPropertyName("TIENDA DESTINO")]
        public string? tienda_destino { get; set; }
        [JsonPropertyName("NOMBRE TIENDA")]
        public string? nombre_tienda { get; set; }
        [JsonPropertyName("TIPO EMBALAJE")]
        public string? tipo_embalaje { get; set; }
        [JsonPropertyName("LPN")]
        public string? lpn { get; set; }
        [JsonPropertyName("Codigo Proveedor")]
        public string? codigo_proveedor { get; set; }
        [JsonPropertyName("Sucursal Destino")]
        public string? sucursal_destino { get; set; }
        [JsonPropertyName("N° de OC")]
        public string? n_de_oc { get; set; }
        //[JsonPropertyName("LPN_1")]
        //public string? lpn_1 { get; set; }
        [JsonPropertyName("Codigo Producto")]
        public string? codigo_producto { get; set; }
        [JsonPropertyName("Cantidad (Und.)")]
        public float? cantidad_und { get; set; }
        public string? o_r { get; set; }
        public string? o_c { get; set; }
        public string? id_destino { get; set; }
        public string? nombre_destino { get; set; }
        public string? destino_cod { get; set; }
        public string? destino_des { get; set; }
        public string? item { get; set; }
        public string? talla { get; set; }
        public string? ean13 { get; set; }
        public string? precio { get; set; }
    }
    //public static class ObjectExtensions
    //{
    //    public static bool IsNull<T>(this T obj)
    //    {
    //        return obj == null || obj.GetType().GetProperties().All(p => p.GetValue(obj) == null);
    //    }
    //}
}