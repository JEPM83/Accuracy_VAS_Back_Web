using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyModel.Printer
{
    public class PrinterRequestWeb
    {
        public string id_almacen { get; set; }
        public string id_printer { get; set; }
    }
    public class PrinterBodyWeb
    {
        public string id_printer { get; set; }
        public string host_name { get; set; }
        public string host_ip { get; set; }
        public string lan_url { get; set; }
        public string extension_file { get; set; }
        public string state { get; set; }
        public string id_almacen { get; set; }
    }
    /*listar correlativo impresion LPN*/
    public class RequestLPNWeb {
        public string id_almacen { get; set; }
        public string id_printer { get; set; }
        public int cantidad { get; set; }
        public string usuario_creacion { get; set; }
    }
    public class BodyLPNWeb
    {
        public string title { get; set; }
        public string message { get; set; }
        public string type { get; set; }
        public int statuscode { get; set; }
    }
    /*registrar LPN*/
    public class RequestCorrelativoLPNWeb
    {
        public string id_almacen { get; set; }
    }
    public class BodyCorrelativoLPNWeb
    {
        public string lpn { get; set; }
    }
    /*Generacion de bultos convertidora ITSANET*/
    public class RequestGenerabultos
    {
        public string id_almacen { get; set; }
        public int id_printer { get; set; }
        public string numero_orden_compra { get; set; }
        public string usuario_creacion { get; set; }
        public bool? impresion { get; set; }
        public string? documento { get; set; }
        public string? numero_item { get; set; }
        public int? cantidad_impresion { get; set; }
    }
    public class BodyGenerabultos
    {
        public string title { get; set; }
        public string message { get; set; }
        public int type { get; set; }
        public int statuscode { get; set; }
    }
    /*Cabecera bultos - itsanet*/
    public class RequestCabeceraBultos
    {
        public string id_almacen { get; set; }
        public string numero_orden_compra { get; set; }
        public string? documento { get; set; }
        public string? numero_item { get; set; }
    }
    public class BodyCabeceraBultos
    {
        public int id_bulto { get; set; }
        public string id_almacen { get; set; }
        public string numero_orden_compra { get; set; }
        public string? linea { get; set; }
        public string numero_bulto { get; set; }
        public float cantidad { get; set; }
        public float cartones { get; set; }
        public string destino { get; set; }
        public string nota { get; set; }
        public string curva { get; set; }
    }
    /*Detalle bultos - itsanet*/
    public class RequestDetalleBultos
    {
        public string id_almacen { get; set; }
        public int id_bulto { get; set; }
    }
    public class BodyDetalleBultos
    {
        public int id_bulto_detalle { get; set; }
        public int id_bulto { get; set; }
        public string id_almacen { get; set; }
        public string numero_orden_compra { get; set; }
        public string numero_item { get; set; }
        public string? ean13 { get; set; }
        public float cantidad { get; set; }
        public string? color { get; set; }
        public string? talla { get; set; }
    }
    /*Generacion de bultos - itsanet - textil*/
    public class RequestBultoxBulto {
        public string id_almacen { get; set; }
        public int id_printer { get; set; }
        public int cantidad { get; set; }
        public string numero_item { get; set; }
        public string talla { get; set; }
        public string ean { get; set; }
        public int cantidad_bulto { get; set; }
        public string importacion { get; set; }
        public string numero_orden_compra { get; set; } //DOCUMENTO
        public string destino { get; set; }
        public string usuario_creacion { get; set; }
    }
    public class ResponseBultoxBulto
    {
        public string title { get; set; }
        public string message { get; set; }
        public int type { get; set; }
        public int statuscode { get; set; }
    }
}
