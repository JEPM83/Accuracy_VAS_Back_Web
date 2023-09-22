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
}
