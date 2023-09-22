using AccuracyData.ObjectosDA;
using AccuracyModel.Inbound;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace AccuracyData.InboundDA
{
    public class PurchaseOrderWebDA
    {
        static string fechaFormatoGlobal = "yyyy-MM-dd HH:mm:ss";
        public List<PurchaseOrderBodyWeb> SP_INBOUND_WEB_GET_ORDER(PurchaseOrderRequestWeb obj, string cnx)
        {
            PurchaseOrderRequestWeb ordenC = new PurchaseOrderRequestWeb();
            List<PurchaseOrderBodyWeb> orderList = new List<PurchaseOrderBodyWeb>();
            try
            {
                ordenC.id_almacen = obj.id_almacen;
                ordenC.numero_orden_compra = obj.numero_orden_compra;
                ordenC.id_tipo = obj.id_tipo;
                ordenC.fecha_inicial = obj.fecha_inicial;
                ordenC.fecha_final = obj.fecha_final;
                ordenC.nombre_proveedor = obj.nombre_proveedor;
                ordenC.estado = obj.estado;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_INBOUND_ORDER, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.VarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@numero_orden_compra", SqlDbType.VarChar).Value = obj.numero_orden_compra;
                    cmd.Parameters.Add("@id_tipo", SqlDbType.Int).Value = obj.id_tipo;
                    cmd.Parameters.Add("@fecha_inicial", SqlDbType.NVarChar).Value = obj.fecha_inicial;
                    cmd.Parameters.Add("@fecha_final", SqlDbType.NVarChar).Value = obj.fecha_final;
                    cmd.Parameters.Add("@nombre_proveedor", SqlDbType.NChar).Value = obj.nombre_proveedor;
                    cmd.Parameters.Add("@estado", SqlDbType.NChar).Value = obj.estado;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var Order = new PurchaseOrderBodyWeb();
                        Order.numero_orden_compra = sqlReader["NUMERO_ORDEN_COMPRA"].ToString(); ;
                        Order.tipo_orden = sqlReader["TIPO_ORDEN"].ToString();
                        Order.estado = sqlReader["ESTADO"].ToString();
                        Order.nombre_proveedor = sqlReader["NOMBRE_PROVEEDOR"].ToString();
                        Order.almacen = sqlReader["ALMACEN"].ToString();

                        DateTime? fecha_entrega_estimada = sqlReader.IsDBNull(sqlReader.GetOrdinal("FECHA_ENTREGA_ESTIMADA")) ? null : (DateTime?)sqlReader.GetDateTime(sqlReader.GetOrdinal("FECHA_ENTREGA_ESTIMADA"));
                        string fechaFormateada = fecha_entrega_estimada.HasValue ? fecha_entrega_estimada.Value.ToString(fechaFormatoGlobal) : string.Empty;
                        Order.fecha_entrega_estimada = fechaFormateada;
                       
                        Order.lineas = sqlReader["LINEAS"].ToString();
                        Order.unidades = float.Parse(sqlReader["UNIDADES"].ToString());
                        orderList.Add(Order);
                    }
                    conn.Close();
                    return orderList;
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                ordenC = null;
            }

        }
        public List<PurchaseOrderDetailBodyWeb> SP_INBOUND_WEB_GET_ORDER_DETAIL(PurchaseOrderDetailRequestWeb obj, string cnx)
        {
            PurchaseOrderDetailRequestWeb ordenC = new PurchaseOrderDetailRequestWeb();
            List<PurchaseOrderDetailBodyWeb> orderList = new List<PurchaseOrderDetailBodyWeb>();
            try
            {
                ordenC.id_almacen = obj.id_almacen;
                ordenC.numero_orden_compra = obj.numero_orden_compra;

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_INBOUND_ORDER_DETAIL, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@numero_orden_compra", SqlDbType.NVarChar).Value = obj.numero_orden_compra;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        var Order = new PurchaseOrderDetailBodyWeb();
                        Order.almacen = sqlReader["ALMACEN"].ToString();
                        Order.numero_orden_compra = sqlReader["NUMERO_ORDEN_COMPRA"].ToString();
                        Order.numero_linea = sqlReader["NUMERO_LINEA"].ToString();
                        Order.item = sqlReader["ITEM"].ToString();
                        Order.descripcion = sqlReader["DESCRIPCION"].ToString();
                        Order.cantidad = float.Parse(sqlReader["CANTIDAD"].ToString());
                        Order.unidad_medida_orden = sqlReader["UNIDAD_MEDIDA_ORDEN"].ToString();
                        Order.nombre_proveedor = sqlReader["NOMBRE_PROVEEDOR"].ToString();
                        Order.documento = sqlReader["DOCUMENTO"].ToString();
                        Order.destino = sqlReader["DESTINO"].ToString();
                        DateTime? fecha_entrega_estimada = sqlReader.IsDBNull(sqlReader.GetOrdinal("FECHA_ENTREGA_ESTIMADA")) ? null : (DateTime?)sqlReader.GetDateTime(sqlReader.GetOrdinal("FECHA_ENTREGA_ESTIMADA"));
                        string fechaFormateada = fecha_entrega_estimada.HasValue ? fecha_entrega_estimada.Value.ToString(fechaFormatoGlobal) : string.Empty;
                        Order.fecha_entrega_estimada = fechaFormateada;

                        orderList.Add(Order);
                    }

                    conn.Close();

                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            return orderList;

        }
        public List<InboundTransactionBodyWeb> SP_INBOUND_WEB_GET_RECEIPT(InboundTransactionRequestWeb obj, string cnx)
        {
            InboundTransactionRequestWeb ordenC = new InboundTransactionRequestWeb();
            List<InboundTransactionBodyWeb> orderList = new List<InboundTransactionBodyWeb>();
            try
            {
                ordenC.id_almacen = obj.id_almacen;
                ordenC.numero_control = obj.numero_control;
                ordenC.id_ubicacion = obj.id_ubicacion;
                ordenC.id_empleado = obj.id_empleado;
                ordenC.fecha_inicial = obj.fecha_inicial;
                ordenC.fecha_final = obj.fecha_final;
                ordenC.numero_item = obj.numero_item;
                ordenC.tipo_transaccion = obj.tipo_transaccion;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_INBOUND_RECEIPT, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@numero_control", SqlDbType.NVarChar).Value = obj.numero_control;
                    cmd.Parameters.Add("@id_ubicacion", SqlDbType.NVarChar).Value = obj.id_ubicacion;
                    cmd.Parameters.Add("@id_empleado", SqlDbType.NVarChar).Value = obj.id_empleado;
                    cmd.Parameters.Add("@fecha_inicial", SqlDbType.NVarChar).Value = obj.fecha_inicial;
                    cmd.Parameters.Add("@fecha_final", SqlDbType.NVarChar).Value = obj.fecha_final;
                    cmd.Parameters.Add("@numero_item", SqlDbType.NVarChar).Value = obj.numero_item;
                    cmd.Parameters.Add("@tipo_transaccion", SqlDbType.NVarChar).Value = obj.tipo_transaccion;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var Order = new InboundTransactionBodyWeb();
                        Order.numero_control = sqlReader["numero_control"].ToString();
                        Order.numero_item = sqlReader["numero_item"].ToString();
                        Order.nombre_item = sqlReader["descripcion"].ToString();
                        Order.familia = sqlReader["familia"].ToString();
                        Order.sub_familia = sqlReader["sub_familia"].ToString();
                        Order.talla = sqlReader["talla"].ToString();
                        Order.numero_lote = sqlReader["numero_lote"].ToString();
                        Order.cantidad_transaccion = decimal.Parse(sqlReader["cantidad_transaccion"].ToString());
                        Order.atributo_generico_1 = sqlReader["atributo_generico_1"].ToString();
                        Order.atributo_generico_2 = sqlReader["atributo_generico_2"].ToString();

                        orderList.Add(Order);
                    }
                    conn.Close();
                    return orderList;
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                ordenC = null;
            }

        }
        public List<DetalleRecepResponse> SP_INBOUND_WEB_GET_RECEIPT(DetalleRecepRequest obj, string cnx)
        {
            DetalleRecepRequest Details_recepcion = new DetalleRecepRequest();
            List<DetalleRecepResponse> detalle_list = null;
            try
            {
                Details_recepcion.orden_compra = obj.orden_compra;
                Details_recepcion.id_almacen = obj.id_almacen;
                Details_recepcion.fecha_inicio = obj.fecha_inicio;
                Details_recepcion.fecha_fin = obj.fecha_fin;
                Details_recepcion.codigo_proveedor = obj.codigo_proveedor;
                Details_recepcion.estado = obj.estado;
                using (SqlConnection conn = new SqlConnection(cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_DETALLE_RECEPCION, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SEC_KEY", SqlDbType.VarChar).Value = string.IsNullOrEmpty(obj.orden_compra) ? (object)DBNull.Value : (object)obj.orden_compra;
                        cmd.Parameters.Add("@ID_ALMACEN", SqlDbType.VarChar).Value = string.IsNullOrEmpty(obj.id_almacen) ? (object)DBNull.Value : (object)obj.id_almacen;
                        cmd.Parameters.Add("@DATE_INICIO", SqlDbType.Date).Value = string.IsNullOrEmpty(obj.fecha_inicio) ? (object)DBNull.Value : (object)DateTime.Parse(obj.fecha_inicio);
                        cmd.Parameters.Add("@DATE_FIN", SqlDbType.Date).Value = string.IsNullOrEmpty(obj.fecha_fin) ? (object)DBNull.Value : (object)DateTime.Parse(obj.fecha_fin);
                        cmd.Parameters.Add("@codigo_proveedor", SqlDbType.NVarChar).Value = obj.codigo_proveedor;
                        cmd.Parameters.Add("@estado", SqlDbType.NChar).Value = obj.estado;
                        conn.Open();
                        SqlDataReader sqlReader = cmd.ExecuteReader();
                        detalle_list = new List<DetalleRecepResponse>();
                        while (sqlReader.Read())
                        {
                            var Detalles = new DetalleRecepResponse();
                            Detalles.id_recepcion = sqlReader["id_recepcion"].ToString();
                            Detalles.cod_proveedor = sqlReader["codigo_proveedor"].ToString();
                            Detalles.nom_proveedor = sqlReader["nombre_proveedor"].ToString();
                            Detalles.orden_compra = sqlReader["numero_orden_compra"].ToString();
                            Detalles.estado = sqlReader["estado"].ToString();
                            Detalles.descripcion_estado = sqlReader["descripcion_estado"].ToString();
                            Detalles.fecha = sqlReader["docduedate"].ToString();
                            Detalles.lineas = sqlReader["lineas"].ToString();
                            detalle_list.Add(Detalles);
                        }
                    }
                }
                return detalle_list;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                Details_recepcion = null;
            }
        }
        public List<DetalleRecep_IDResponse> SP_INBOUND_WEB_GET_RECEIPT_LINE(DetalleRecep_IDRequest obj, string cnx)
        {
            List<DetalleRecep_IDResponse> detalle_list = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_DETALLE_RECEPCION_LINEA, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@RECEIPT_ID", SqlDbType.Int).Value = obj.id_recepcion;

                        conn.Open();
                        SqlDataReader sqlReader = cmd.ExecuteReader();
                        detalle_list = new List<DetalleRecep_IDResponse>();
                        while (sqlReader.Read())
                        {
                            var Detalles = new DetalleRecep_IDResponse();
                            Detalles.id_recepcion = sqlReader["id_recepcion"].ToString();
                            Detalles.id_detalle = sqlReader["id_recepcion_linea"].ToString();
                            Detalles.nom_almacen = sqlReader["almacen"].ToString();
                            Detalles.cod_producto = sqlReader["numero_item"].ToString();
                            Detalles.descrip_producto = sqlReader["descripcion"].ToString();
                            Detalles.cant_esperada = float.Parse(sqlReader["cantidad_esp"].ToString());
                            Detalles.cant_recibida = float.Parse(sqlReader["cantidad_rec"].ToString());
                            Detalles.cant_recepcion = float.Parse(sqlReader["cantidad_rec1"].ToString());
                            Detalles.cant_danada = float.Parse(sqlReader["cantidad_da"].ToString());
                            Detalles.comentario = sqlReader["comentario"].ToString();
                            Detalles.documento = sqlReader["documento"].ToString();
                            detalle_list.Add(Detalles);
                        }
                    }
                }
                return detalle_list;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
        public CloseReceiptResponse SP_INBOUND_WEB_POST_SYNC_PURCHASE_ORDER(CloseReceipRequest obj, string cnx)
        {
            var Detalles = new CloseReceiptResponse();
            try
            {
                using (SqlConnection conn = new SqlConnection(cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_POST_CLOSE_RECEIPT, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SEC_KEY", SqlDbType.VarChar).Value = obj.numero_orden_compra;
                        cmd.Parameters.Add("@STATUS", SqlDbType.Char).Value = obj.estado;
                        cmd.Parameters.Add("@USER_CREATE", SqlDbType.NVarChar).Value = obj.usuario;
                        cmd.Parameters.Add("@RECEIPT_ID", SqlDbType.Int).Value = obj.id_recepcion;
                        conn.Open();
                        SqlDataReader sqlReader = cmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            Detalles.title = "Aviso";
                            Detalles.message = sqlReader["MESSAGE"].ToString();
                            Detalles.type = "0";
                            Detalles.statuscode = 200;
                        }
                    }
                }
                return Detalles;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
        public InsertReceiptResponse SP_INBOUND_WEB_POST_INSERT_PURCHASE_ORDER(InsertReceiptRequest obj, string cnx)
        {
            var Detalles = new InsertReceiptResponse();
            try
            {
                using (SqlConnection conn = new SqlConnection(cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_POST_INSERT_PURCHASE_ORDER, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SEC_KEY", SqlDbType.VarChar).Value = obj.numero_orden_compra;
                        cmd.Parameters.Add("@COD_PROCESS", SqlDbType.Char).Value = obj.id_proceso;
                        cmd.Parameters.Add("@STATUS", SqlDbType.NVarChar).Value = obj.estado;
                        cmd.Parameters.Add("@IDE_USER", SqlDbType.Int).Value = 1; //eliminar luego este campo
                        cmd.Parameters.Add("@USER_CREATE", SqlDbType.NVarChar).Value = obj.usuario;
                        cmd.Parameters.Add("@CARDCODE", SqlDbType.NVarChar).Value = obj.codigo_proveedor;
                        cmd.Parameters.Add("@CARDNAME", SqlDbType.NVarChar).Value = obj.nombre_proveedor;
                        cmd.Parameters.Add("@LINENUM", SqlDbType.Int).Value = obj.numero_linea;
                        cmd.Parameters.Add("@BASEENTRY", SqlDbType.Int).Value = obj.baseentry;
                        cmd.Parameters.Add("@BASELINE", SqlDbType.Int).Value = obj.baseline;
                        cmd.Parameters.Add("@ITEMCODE", SqlDbType.NVarChar).Value = obj.numero_item;
                        cmd.Parameters.Add("@DSCRIPTION", SqlDbType.NVarChar).Value = obj.descripcion;
                        cmd.Parameters.Add("@QUANTITY", SqlDbType.Float).Value = obj.cantidad;
                        cmd.Parameters.Add("@WHSCODE", SqlDbType.NVarChar).Value = obj.id_almacen;
                        cmd.Parameters.Add("@UNITMSR", SqlDbType.NVarChar).Value = obj.unitmsr;
                        cmd.Parameters.Add("@uomcode", SqlDbType.NVarChar).Value = obj.uomcode;
                        cmd.Parameters.Add("@uomconvfactor", SqlDbType.Decimal).Value = obj.uomconvfactor;
                        cmd.Parameters.Add("@DISTNUMBER", SqlDbType.NVarChar).Value = obj.distnumber;
                        cmd.Parameters.Add("@EXPDATE", SqlDbType.Date).Value = obj.expdate;
                        cmd.Parameters.Add("@BINABS", SqlDbType.VarChar).Value = obj.binabs;
                        cmd.Parameters.Add("@id_hu", SqlDbType.VarChar).Value = obj.id_hu;
                        cmd.Parameters.Add("@atributo_01", SqlDbType.NVarChar).Value = obj.atributo01;
                        cmd.Parameters.Add("@atributo_02", SqlDbType.NVarChar).Value = obj.atributo02;
                        cmd.Parameters.Add("@textolibre", SqlDbType.NVarChar).Value = obj.texto_libre;
                        cmd.Parameters.Add("@receipt_id", SqlDbType.Int).Value = obj.id_recepcion;
                        cmd.Parameters.Add("@json_rfid", SqlDbType.NVarChar).Value = JsonConvert.SerializeObject(obj.rfid);
                        conn.Open();
                        SqlDataReader sqlReader = cmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            Detalles.title = "Aviso";
                            Detalles.message = sqlReader["MESSAGE"].ToString();
                            if (sqlReader["STATE"].ToString() == "0")
                            {
                                Detalles.type = 0;
                                Detalles.statuscode = 200;
                            }
                            else {
                                Detalles.type = 3;
                                Detalles.statuscode = 400;
                            }
                            Detalles.id_recepcion = int.Parse(sqlReader["IDE_OPDN"].ToString());
                        }
                    }
                }
                return Detalles;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
        public List<PurchaseOrderEanResponse> SP_INBOUND_WEB_GET_ORDER_DETAIL_EAN(PurchaseOrderEanRequest obj, string cnx)
        {
            PurchaseOrderEanRequest ordenC = new PurchaseOrderEanRequest();
            List<PurchaseOrderEanResponse> orderList = new List<PurchaseOrderEanResponse>();
            try
            {
                ordenC.id_almacen = obj.id_almacen;
                ordenC.numero_orden_compra = obj.numero_orden_compra;

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_INBOUND_ORDER_DETAIL_EAN, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@numero_orden_compra", SqlDbType.NVarChar).Value = obj.numero_orden_compra;
                    cmd.Parameters.Add("@numero_linea", SqlDbType.NVarChar).Value = obj.numero_linea;
                    cmd.Parameters.Add("@numero_item", SqlDbType.NVarChar).Value = obj.numero_item;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        var Order = new PurchaseOrderEanResponse();
                        Order.numero_linea = sqlReader["NUMERO_LINEA"].ToString();
                        Order.numero_secuencia = int.Parse(sqlReader["NUMERO_SECUENCIA"].ToString());
                        Order.numero_item = sqlReader["ITEM"].ToString();
                        Order.descripcion = sqlReader["DESCRIPCION"].ToString();
                        Order.ean = sqlReader["EAN"].ToString();
                        Order.talla = sqlReader["TALLA"].ToString();
                        Order.color = sqlReader["COLOR"].ToString();
                        Order.piezas = int.Parse(sqlReader["PIEZAS"].ToString());
                        Order.cartones = int.Parse(sqlReader["CARTONES"].ToString());
                        Order.destino = sqlReader["DESTINO"].ToString();
                        Order.documento = sqlReader["DOCUMENTO"].ToString();
                        orderList.Add(Order);
                    }
                    conn.Close();
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            return orderList;

        }
        public List<ReceiptLineEanResponse> SP_INBOUND_WEB_GET_RECEIPT_DETAIL_EAN(ReceiptLineEanRequest obj, string cnx)
        {
            ReceiptLineEanRequest ordenC = new ReceiptLineEanRequest();
            List<ReceiptLineEanResponse> orderList = new List<ReceiptLineEanResponse>();
            try
            {
                ordenC.id_almacen = obj.id_almacen;
                ordenC.numero_orden_compra = obj.numero_orden_compra;

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_INBOUND_RECEIPT_DETAIL_EAN, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@numero_orden_compra", SqlDbType.NVarChar).Value = obj.numero_orden_compra;
                    cmd.Parameters.Add("@id_recepcion", SqlDbType.Int).Value = obj.id_recepcion;
                    cmd.Parameters.Add("@numero_linea", SqlDbType.NVarChar).Value = obj.numero_linea;
                    cmd.Parameters.Add("@numero_item", SqlDbType.NVarChar).Value = obj.numero_item;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        var Order = new ReceiptLineEanResponse();
                        Order.id_almacen = sqlReader["id_almacen"].ToString();
                        Order.numero_orden_compra = sqlReader["numero_orden_compra"].ToString();
                        Order.id_recepcion = int.Parse(sqlReader["id_recepcion"].ToString());
                        Order.linea_base = sqlReader["linea_base"].ToString();
                        Order.numero_linea = sqlReader["numero_linea"].ToString();
                        Order.numero_item = sqlReader["numero_item"].ToString();
                        Order.descripcion = sqlReader["descripcion"].ToString();
                        Order.cantidad_pedida = float.Parse(sqlReader["cantidad_pedida"].ToString());
                        Order.rfid = sqlReader["rfid"].ToString();
                        Order.ean = sqlReader["ean"].ToString();
                        Order.empresa = sqlReader["empresa"].ToString();
                        Order.serie = sqlReader["serie"].ToString();
                        Order.estado = sqlReader["estado"].ToString();
                        Order.usuario = sqlReader["usuario"].ToString();
                        Order.destino = sqlReader["destino"].ToString();
                        Order.talla = sqlReader["talla"].ToString();
                        orderList.Add(Order);
                    }
                    conn.Close();
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            return orderList;

        }
    }
}
