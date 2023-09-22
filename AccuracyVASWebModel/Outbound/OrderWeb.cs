using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyModel.Outbound
{
    public class OutboundTransactionRequestWeb
    {
        public string id_almacen { get; set; }
        public string numero_control { get; set; }
        public string id_ubicacion { get; set; }
        public string id_empleado { get; set; }
        public string fecha_inicial { get; set; }
        public string? fecha_final { get; set; }
        public string? numero_item { get; set; }
        public string? tipo_transaccion { get; set; }
    }
    public class OutboundTransactionBodyWeb
    {
        public string id { get; set; }
        public string tipo_transaccion { get; set; }
        public string inicio_fecha { get; set; }
        public string fin_fecha { get; set; }
        public string id_empleado { get; set; }
        public string numero_control { get; set; }
        public string id_almacen { get; set; }
        public string id_ubicacion { get; set; }
        public string numero_item { get; set; }
        public string codigo_item { get; set; }
        public string numero_lote { get; set; }
        public string talla { get; set; }
        public decimal cantidad_transaccion { get; set; }
        public string tipo_mov { get; set; }
        public string atributo_generico_1 { get; set; }
        public string atributo_generico_2 { get; set; }
        public string nombre_item { get; set; }
        public string familia { get; set; }
        public string sub_familia { get; set; }

    }
    public class OrderPickingRequest
    {
        public string id_almacen { get; set; }
        public string orden { get; set; }
        public string tipo { get; set; }
        public string urgencia { get; set; }
        public string? fecha_pedido_inicial { get; set; }
        public string? fecha_pedido_final { get; set; }
        public string status { get; set; }
    }
    public class OrderPickingBody
    {
        public int id_pedido { get; set; }
        public string id_almacen { get; set; }
        public string cliente { get; set; }
        public string orden { get; set; }
        public string tipo { get; set; }
        public string urgencia { get; set; }
        public string? oc_cliente { get; set; }
        public string? fecha_pedido_inicial { get; set; }
        public string? fecha_pedido_final { get; set; }
        public DateTime fecha_liberacion { get; set; }
        public decimal peso { get; set; }
        public int lineas { get; set; }
        public decimal cantidad_solicitada { get; set; }
        public decimal avance_picking { get; set; }
        public string? envio { get; set; }
        public string status { get; set; }
        public string? fecha_despacho_objetivo { get; set; }
        public string? fecha_despacho_real { get; set; }
    }
    public class Order_DetailRequest
    {
        public int id_pedido { get; set; }
    }
    public class Order_DetailBody
    {
        public int id_pedido { get; set; }
        public string orden { get; set; }
        public int linea { get; set; }
        public string item { get; set; }
        public string descripcion { get; set; }
        public string lote { get; set; }
        public decimal cantidad { get; set; }
        public string unidad { get; set; }
        public string atributo_01 { get; set; }
        public string atributo_02 { get; set; }
        public string almacen { get; set; }
    }
    public class OrderPickingUpdateRequest {
        public int id_pedido { get; set; }
        public string campo { get; set; }
        public string valor { get; set; }
    }
  
}