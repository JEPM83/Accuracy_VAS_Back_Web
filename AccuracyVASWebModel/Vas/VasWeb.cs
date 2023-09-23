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
    public class OrderPedidoRequest {
        public string id_almacen { get; set; }
        public string? numero_pedido { get; set; }
    }
    public class OrderPedidoResponse { 
        public string cliente { get; set; }
        public string tienda { get; set; }
        public string numero_pedido { get; set; }
        public float avance_vas { get; set; }
        public int lineas_produccion { get; set; }
        public string id_hu { get; set; }
        public string categoria_inventario { get; set; }
        public string color_fondo { get; set; }
    }
    public class OrderPedidoDetailPickingRequest
    {
        public string id_almacen { get; set; }
        public string numero_pedido { get; set; }
        public string? id_hu { get; set; }
        public string? numero_item { get; set; }
    }
    public class OrderPedidoDetailPickingResponse
    {
        public string numero_item { get; set; }
        public string descripcion { get; set; }
        public string unidad_medida { get; set; }
        public float cantidad_picking { get; set; }
        public string categoria_inventario { get; set; }
        public string subcategoria_inventario { get; set; }
        public string atributo_generico_1 { get; set; }
        public string atributo_generico_2 { get; set; }
    }
    public class TaskRequest
    {
        public string id_almacen { get; set; }
        public string numero_pedido { get; set; }
        public string? id_hu { get; set; }
        public string? numero_item { get; set; }
    }
    public class TaskResponse
    {
        //public string numero_pedido {get; set; }
        public string cliente { get; set; }
        public string categoria_inventario { get; set; }
        public int id_agrupador { get; set; }
        public string descripcion_agrupador { get; set; }
        public int id_tarea { get; set; }
        public int secuencia{ get; set; }
        public string descripcion_tarea { get; set; }
        public string comentario { get; set; }
    }
    public class RootTaskObject
    {
        //public string numero_pedido { get; set; }
        public string cliente { get; set; }
        public string categoria_inventario { get; set; }
        public List<DetalleTaskGroup> grupo { get; set; }
    }
    public class DetalleTaskGroup { 
        public int id_agrupador { get; set; }
        public string descripcion_agrupador { get; set; }
        public List<SubDetalleTaskDetail> tareas { get; set; }
    }
    public class SubDetalleTaskDetail { 
        public int id_tarea { get; set; }
        public int secuencia { get; set; }
        public string descripcion_tarea { get; set; }
        public string comentario { get; set; }
    }
    public class InicioTareaRequest
    {
        public string id_almacen { get; set; }
        public string numero_pedido { get; set; }
        public string? id_hu { get; set; }
        public string? numero_item { get; set; }
        public int? id_tarea { get; set; }
    }
    public class InicioTareaResponse
    {
        public string type { get; set; }
        public string message { get; set; }
        public string tittle { get; set; }
    }
}