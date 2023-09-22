using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyModel.Inbound
{
    public class InboundTransactionRequestWeb
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
    public class InboundTransactionBodyWeb
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
    #region Cabecera Order
    public class PurchaseOrderRequestWeb
    {
        public string id_almacen { get; set; }
        public string? numero_orden_compra { get; set; }
        public int? id_tipo { get; set; }
        public string? fecha_inicial { get; set; }
        public string? fecha_final { get; set; }
        public string? nombre_proveedor { get; set; }
        public string? estado { get; set; }
    }
    public class PurchaseOrderBodyWeb
    {
        public string numero_orden_compra { get; set; }
        public string? tipo_orden { get; set; }
        public string? estado { get; set; }
        public string nombre_proveedor { get; set; }
        public string almacen { get; set; }
        public string? fecha_entrega_estimada { get; set; }
        public string? lineas { get; set; }
        public float? unidades { get; set; }
    }
    #endregion
    #region Detalle Order
    public class PurchaseOrderDetailRequestWeb
    {
        public string id_almacen { get; set; }
        public string? numero_orden_compra { get; set; }
    }
    public class PurchaseOrderDetailBodyWeb
    {
        public string almacen { get; set; }
        public string numero_orden_compra { get; set; }
        public string numero_linea { get; set; }
        public string item { get; set; }
        public string descripcion { get; set; }
        public float cantidad { get; set; }
        public string unidad_medida_orden { get; set; }
        public string nombre_proveedor { get; set; }
        public string fecha_entrega_estimada { get; set; }
        public string documento { get; set; }
        public string destino { get; set; }
    }
    #endregion
    public class DetalleRecepRequest
    {
        public string id_almacen { get; set; }
        public string? orden_compra { get; set; }
        public string? fecha_inicio { get; set; }
        public string? fecha_fin { get; set; }
        public string? codigo_proveedor { get; set; }
        public string? estado { get; set; }
    }
    public class DetalleRecepResponse
    {
        public string id_recepcion { get; set; }
        public string cod_proveedor { get; set; }
        public string nom_proveedor { get; set; }
        public string orden_compra { get; set; }
        public string estado { get; set; }
        public string descripcion_estado { get; set; }
        public string fecha { get; set; }
        public string lineas { get; set; }
    }
    public class DetalleRecep_IDRequest
    {
        public int id_recepcion { get; set; }
    }
    public class DetalleRecep_IDResponse
    {
        public string id_recepcion { get; set; }
        public string id_detalle { get; set; }
        public string nom_almacen { get; set; }
        public string cod_producto { get; set; }
        public string descrip_producto { get; set; }
        public float cant_esperada { get; set; }
        public float cant_recibida { get; set; }
        public float cant_recepcion { get; set; }
        public float cant_danada { get; set; }
        public string comentario { get; set; }
        public string documento { get; set; }
    }
    /*Insertar Recepcion*/
    public class InsertReceiptRequest {
        public string numero_orden_compra { get; set; } //sec_key
        public string id_proceso { get; set; } //cod_process
        public string estado { get; set; }
        public string usuario { get; set; } //ide_user & user_create
        public string codigo_proveedor { get; set; } //cardcode
        public string nombre_proveedor { get; set; } //cardname
        public int numero_linea { get; set; }
        public int baseentry { get; set; }
        public int baseline { get; set; }
        public string numero_item { get; set; }
        public string descripcion { get; set; }
        public float cantidad { get; set; }
        public string id_almacen { get; set; }
        public string unitmsr { get; set; }
        public string uomcode { get; set; }
        public float uomconvfactor { get; set; }
        public string distnumber { get; set; }
        public string? expdate { get; set; }
        public string binabs { get; set; }
        public string? id_hu { get; set; }
        public string atributo01 { get; set; }
        public string atributo02 { get; set; }
        public string texto_libre { get; set; }
        public int? id_recepcion { get; set; }
        public eRfid[]? rfid { get; set; }
    }
    public class eRfid { 
        public string rfid { get; set; }
        public string ean13 { get; set; }
        public string empresa { get; set; }
        public string numero_serie { get; set; }
    }
    public class InsertReceiptResponse {
        public string title { get; set; }
        public string message { get; set; }
        public int type { get; set; }
        public int statuscode { get; set; }
        public int? id_recepcion { get; set; }
    }
    /*Sincronizar recepcion*/
    public class CloseReceipRequest {
        public string numero_orden_compra { get; set; }
        public string estado { get; set; }
        public string usuario { get; set; }
        public int? id_recepcion { get; set; }
    }
    public class CloseReceiptResponse {
        public string title { get; set; }
        public string message { get; set; }
        public string type { get; set; }
        public int statuscode { get; set; }
    }
    /*Listar EAN13 de una orden cargada -- PARA ITSANET*/
    public class PurchaseOrderEanRequest
    {
        public string id_almacen { get; set; }
        public string numero_orden_compra { get; set; }
        public string? numero_linea { get; set; }
        public string? numero_item { get; set; }
    }
    public class PurchaseOrderEanResponse
    {
        public string numero_linea { get; set; }
        public int numero_secuencia { get; set; }
        public string numero_item { get; set; }
        public string descripcion { get; set; }
        public string ean { get; set; }
        public string talla { get; set; }
        public string color { get; set; }
        public int piezas { get; set; }
        public int cartones { get; set; }
        public string destino { get; set; }
        public string documento { get; set; }
    }
    /*Detalle de recepcion por EAN*/
    public class ReceiptLineEanRequest {
        public string id_almacen { get; set; }
        public string numero_orden_compra { get; set; }
        public int? id_recepcion { get; set; }
        public string? numero_linea { get; set; }
        public string? numero_item { get; set; }
    }
    public class ReceiptLineEanResponse
    {
        public string id_almacen { get; set; }
        public string numero_orden_compra { get; set; }
        public int id_recepcion { get; set; }
        public string linea_base { get; set; }
        public string numero_linea { get; set; }
        public string numero_item { get; set; }
        public string descripcion { get; set; }
        public float cantidad_pedida { get; set; }
        public string rfid { get; set; }
        public string ean { get; set; }
        public string empresa { get; set; }
        public string serie { get; set; }
        public string estado { get; set; }
        public string usuario { get; set; }
        public string destino { get; set; }
        public string talla { get; set; }
    }
}
