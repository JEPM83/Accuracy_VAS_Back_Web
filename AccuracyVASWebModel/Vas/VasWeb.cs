﻿using System;
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
        public string? id_hu { get; set; }
        public string categoria_inventario { get; set; }
        public string color_fondo { get; set; }
        public string medida { get; set; }
    }
    public class  NotifyOrderRequest
    {
        public string id_almacen { get; set; }
        public string numero_pedido { get; set; }
        public string linea_produccion { get; set; }
        public string? id_hu { get; set; }
        public string usuario { get; set; }
    }
    public class NotifyOrderResponse
    {
        public string type { get; set; }
        public string message { get; set; }
        public string tittle { get; set; }
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
        public string? atributo_generico_1 { get; set; }
        public string? atributo_generico_2 { get; set; }
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
        public string? comentario { get; set; }
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
        public string linea_produccion { get; set; }
        public string? id_hu { get; set; }
        public string? numero_item { get; set; }
        public int? id_tarea { get; set; }
        public string usuario { get; set; }
    }
    public class InicioTareaResponse
    {
        public string type { get; set; }
        public string message { get; set; }
        public string tittle { get; set; }
    }
    public class PackingdetailRequest {
        public string id_almacen { get; set; }
        public string numero_pedido { get; set; }
        public string linea_produccion { get; set; }
        public string? id_hu { get; set; }
    }
    public class PackingdetailResponse
    {
        public string lpn { get; set; }
        public string numero_item { get; set; }
        public string descripcion { get; set; }
        public float cantidad { get; set; }
        public string unidad_medida { get; set; }
        public string? talla_b2b { get; set; }
    }
    public class LPNvalidateRequest
    {
        public string id_almacen { get; set; }
        public string lpn { get; set; }
    }
    public class LPNvalidaResponse {
        public string type { get; set; }
        public string message { get; set; }
        public string tittle { get; set; }
    }
    public class LPNSKURequest
    {
        public string id_almacen { get; set; }
        public string numero_pedido { get; set; }
        public string linea_produccion { get; set; }
        public string? id_hu { get; set; }
        public string lpn { get; set; }
        public string numero_item { get; set; }
        public float cantidad { get; set; }
        public string usuario { get; set; }
    }
    public class LPNSKUResponse
    {
        public string type { get; set; }
        public string message { get; set; }
        public string tittle { get; set; }
    }
    public class DeleteVasRequest
    {
        public string id_almacen { get; set; }
        public string numero_pedido { get; set; }
        public string linea_produccion { get; set; }
        public string? id_hu { get; set; }
        public string lpn { get; set; }
        public string numero_item { get; set; }
        public string usuario { get; set; }
    }
    public class DeleteVasResponse
    {
        public string type { get; set; }
        public string message { get; set; }
        public string tittle { get; set; }
    }
    public class FinTareaRequest
    {
        public string id_almacen { get; set; }
        public string numero_pedido { get; set; }
        public string linea_produccion { get; set; }
        public string? id_hu { get; set; }
        public string? numero_item { get; set; }
        public int? id_tarea { get; set; }
        public string? usuario { get; set; }
    }
    public class FinTareaResponse
    {
        public string type { get; set; }
        public string message { get; set; }
        public string tittle { get; set; }
    }
    public class InicioIncidenciaRequest {
        public string id_almacen { get; set; }
        public string numero_pedido { get; set; }
        public string linea_produccion { get; set; }
        public string? id_hu { get; set; }
        public int id_razon { get; set; }
        public string descripcion_razon { get; set; }
        public string observacion { get; set; }
        public string usuario { get; set; }
    }
    public class InicioIncidenciaResponse
    {
        public string type { get; set; }
        public string message { get; set; }
        public string tittle { get; set; }
    }
    public class FinIncidenciaRequest
    {
        public string id_almacen { get; set; }
        public string numero_pedido { get; set; }
        public string linea_produccion { get; set; }
        public string? id_hu { get; set; }
        public int id_razon { get; set; }
        public string descripcion_razon { get; set; }
        public string usuario { get; set; }
    }
    public class FinIncidenciaResponse
    {
        public string type { get; set; }
        public string message { get; set; }
        public string tittle { get; set; }
    }
    public class ListaNotificacionRequest {
        public string id_almacen { get; set; }
        public string proceso { get; set; }
    }
    public class ListaNotificacionResponse
    {
        public int id_notificacion { get; set; }
        public string proceso { get; set; }
        public string linea_produccion { get; set; }
        public string usuario { get; set; }
        public int id_razon { get; set; }
        public string descripcion_razon { get; set; }
        public string observacion { get; set; }
        public int minutos_atras_reportado { get; set; }
    }
    public class ActualizacionNotificacionRequest
    {
        public string id_almacen { get; set; }
        public string proceso { get; set; }
        public int id_notificacion { get; set; }
        public string usuario { get; set; }
    }
    public class ActualizacionNotificacionResponse {
        public string type { get; set; }
        public string message { get; set; }
        public string tittle { get; set; }
    }
    public class PanelLineaRequest { 
        public string id_almacen { get; set; }
    }
    public class PanelLineaResponse { 
        public string linea_produccion { get; set; }
        public string nombres { get; set;}
        public string numero_pedido { get; set; }
        public float cantidad_vas { get; set; }
        public int estado { get; set; }
        public string semaforo { get; set; }
        public string tiempo { get; set; }
        public int fila { get; set; }
        public int columna { get; set; }
        public int grupo { get; set; }
    }
    public class PanelOrdenRequest
    {
        public string id_almacen { get; set; }
    }
    public class PanelOrdenResponse
    {
        public string numero_pedido { get; set; }
        public float cantidad_picada { get; set; }
        public float cantidad_vas { get; set; }
        public int estado { get; set; }
        public string color_barra { get;set; }
        public int? total_linea_produccion { get; set; }
        public int? total_usuario { get; set; }
    }
    /*SUPERVISOR*/
    public class OrdenesVasRequest
    {
        public string id_almacen { get; set; }
        public string numero_pedido { get; set; }
        public string? fecha_inicial_despacho { get; set; }
        public string? fecha_final_despacho { get; set; }
    }
    public class OrdenesVasResponse
    {
        public int id_pedido { get; set; }
        public string proveedor { get; set; }
        public string numero_pedido { get; set; }
        public string id_destino { get; set; }
        public string destino { get; set; }
        public string familia_producto { get; set; }
        public float cantidad{ get; set; }
        public float avance { get; set; }
        public string fecha_despacho { get; set; }
        public string estado { get; set; }
    }
    public class UsuariosVasRequest
    {
        public string id_almacen { get; set; }
        public string numero_pedido { get; set; }
        public string? fecha_inicial_despacho { get; set; }
        public string? fecha_final_despacho { get; set; }
    }
    public class UsuariosVasResponse
    {
        public string colaborador { get; set; }
        public string linea_produccion { get; set; }
        public string estado { get; set; }
        public string numero_pedido { get; set; }
        public string destino { get; set; }
        public string categoria_inventario { get; set; }
    }
    public class B2BVasRequest {
        public string id_almacen { get; set; }
        public string numero_pedido { get; set; }
        public string? fecha_inicial_despacho { get; set; }
        public string? fecha_final_despacho { get; set; }
    }
    public class B2BVasResponse { 
        public string o_r { get; set; }
        public string o_c { get; set; }
        public string id_destino { get; set; }
        public string nombre_destino { get; set; }
        public string destino_cod { get; set; }
        public string destino_des { get; set; }
        public string sku { get; set; }
        public string item { get; set; }
        public string talla { get; set; }
        public string cantidad { get; set; }
        public string ean13 { get; set; }
        public string? precio { get; set; }
        public string lpn { get; set; }
    }
    public class PrinteLpnRequest { 
        public string id_almacen { get; set; }
        public int id_printer { get; set; }
        public string modelo { get; set; }
        public string numero_pedido { get; set;}
        public string id_destino { get; set; }
        public string linea { get; set; }
        public string lote { get; set; }
        public string cita { get; set; }
        public string factura { get; set; }
        public int cantidad { get; set; }
        public string usuario_creacion { get; set; }
    }
    public class PrinterLpnResponse {
        public string type { get; set; }
        public string message { get; set; }
        public string tittle { get; set; }
    }
    public class UpdateStateRequest { 
        public string id_almacen { get; set; }
        public string numero_pedido { get; set; }
        public string estado { get; set; }
        public string usuario { get; set; }
    }
    public class UpdateStateResponse
    {
        public string type { get; set; }
        public string message { get; set; }
        public string tittle { get; set; }
    }
    public class DestinoRequest {
        public string id_almacen { get; set; }
        public string id_cliente { get; set; }
        public string? usuario { get; set; }
    }
    public class DestinoResponse
    {
        public string id_destino { get; set; }
        public string nombre_destino { get; set;}
    }
    public class ImprimirLpnRequest {
        public string id_almacen { get; set; }
        public string id_cliente { get; set; }
        public string id_destino { get; set; }
        public int id_printer { get; set; }
        public int cantidad { get; set; }
        public string? usuario_creacion { get; set; }
    }
    public class ImprimirLpnResponse
    {
        public string type { get; set; }
        public string message { get; set; }
        public string tittle { get; set; }
    }
    public class CorrelativoLpnRequest {
        public string id_almacen { get; set; }
        public string id_cliente { get; set; }
        public string id_destino { get; set; }
    }
    public class CorrelativoLpnResponse
    {
        public string prefijo { get; set; }
        public string correlativo { get; set; }
    }
    public class B2BAdvanceRequest {
        public string id_almacen { get; set; }
        public string id_cliente { get; set; }
        public string id_destino { get; set; }
        public string numero_pedido { get; set; }
    }
    public class B2BAdvanceResponse
    {
        public string numero_pedido { get; set; }
        public string destino_des { get; set; }
        public string numero_item { get; set; }
        public string talla { get; set; }
        public float cantidad { get; set; }
    }
}