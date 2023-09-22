using AccuracyData.ObjectosDA;
using AccuracyModel.Outbound;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccuracyModel.Inbound;
using AccuracyModel.Inventory;
using AccuracyModel.Security;
using System.Reflection;

namespace AccuracyData.OutboundDA
{
    public class OrderWebDA
    {
        static string fechaFormatoGlobal = "yyyy-MM-dd HH:mm:ss";
        public List<OrderPickingBody> SP_OUTBOUND_WEB_GET_ORDER(OrderPickingRequest obj, string cnx)
        {
            OrderPickingRequest ordenC = new OrderPickingRequest();
            List<OrderPickingBody> orderList = new List<OrderPickingBody>();
            try
            {
                ordenC.id_almacen = obj.id_almacen;
                ordenC.orden = obj.orden;
                ordenC.tipo = obj.tipo;
                ordenC.fecha_pedido_inicial = obj.fecha_pedido_inicial;
                ordenC.fecha_pedido_final = obj.fecha_pedido_final;
                ordenC.urgencia = obj.urgencia;
                ordenC.status = obj.status;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_OUTBOUND_ORDER, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.VarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@numero_orden", SqlDbType.VarChar).Value = obj.orden;
                    cmd.Parameters.Add("@id_tipo", SqlDbType.Int).Value = obj.tipo;
                    cmd.Parameters.Add("@fecha_inicial", SqlDbType.NVarChar).Value = obj.fecha_pedido_inicial;
                    cmd.Parameters.Add("@fecha_final", SqlDbType.NVarChar).Value = obj.fecha_pedido_final;
                    cmd.Parameters.Add("@urgencia", SqlDbType.NChar).Value = obj.urgencia;
                    cmd.Parameters.Add("@estado", SqlDbType.NChar).Value = obj.status;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var Order = new OrderPickingBody();
                        Order.id_pedido = int.Parse(sqlReader["id"].ToString());
                        Order.id_almacen = sqlReader["id_almacen"].ToString();
                        Order.cliente = sqlReader["cliente"].ToString();
                        Order.orden = sqlReader["orden"].ToString();
                        Order.tipo = sqlReader["tipo"].ToString();
                        Order.urgencia = sqlReader["urgencia"].ToString();
                        Order.lineas = int.Parse(sqlReader["lineas"].ToString());
                        Order.cantidad_solicitada = decimal.Parse(sqlReader["cantidad_solicitada"].ToString());
                        Order.avance_picking = decimal.Parse(sqlReader["avance_picking"].ToString());
                        Order.envio = sqlReader["envio"].ToString();
                        Order.status = sqlReader["status"].ToString();

                        DateTime? fecha_despacho_objetivo = sqlReader.IsDBNull(sqlReader.GetOrdinal("fecha_despacho_objetivo")) ? null : (DateTime?)sqlReader.GetDateTime(sqlReader.GetOrdinal("fecha_despacho_objetivo"));
                        string fechaFormateada = fecha_despacho_objetivo.HasValue ? fecha_despacho_objetivo.Value.ToString(fechaFormatoGlobal) : string.Empty;
                        Order.fecha_despacho_objetivo = fechaFormateada;

                        DateTime? fecha_despacho_real = sqlReader.IsDBNull(sqlReader.GetOrdinal("fecha_despacho_real")) ? null : (DateTime?)sqlReader.GetDateTime(sqlReader.GetOrdinal("fecha_despacho_real"));
                        fechaFormateada = fecha_despacho_real.HasValue ? fecha_despacho_real.Value.ToString(fechaFormatoGlobal) : string.Empty;
                        Order.fecha_despacho_real = fechaFormateada;

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
        public List<Order_DetailBody> SP_OUTBOUND_WEB_GET_ORDER_DETAIL(Order_DetailRequest obj, string cnx)
        {
            Order_DetailRequest ordenC = new Order_DetailRequest();
            List<Order_DetailBody> orderList = new List<Order_DetailBody>();
            try
            {
                ordenC.id_pedido = obj.id_pedido;

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_OUTBOUND_ORDER_DETAIL, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_pedido", SqlDbType.Int).Value = obj.id_pedido;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    
                    while (sqlReader.Read())
                    {
                        var Order = new Order_DetailBody();
                        Order.orden = sqlReader["orden"].ToString();
                        Order.linea = int.Parse(sqlReader["linea"].ToString());
                        Order.item = sqlReader["item"].ToString();
                        Order.descripcion = sqlReader["descripcion"].ToString();
                        Order.lote = sqlReader["lote"].ToString();
                        Order.cantidad = decimal.Parse(sqlReader["cantidad"].ToString());
                        Order.atributo_01 = sqlReader["atributo_01"].ToString();
                        Order.atributo_02 = sqlReader["atributo_02"].ToString();
                        Order.unidad = sqlReader["unidad"].ToString();
                        Order.almacen = sqlReader["almacen"].ToString();
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
        public List<OrderPickingBody> SP_OUTBOUND_WEB_POST_UPDATE_ORDER(OrderPickingUpdateRequest obj, string cnx)
        {
            OrderPickingUpdateRequest ordenC = new OrderPickingUpdateRequest();
            List<OrderPickingBody> orderList = new List<OrderPickingBody>();
            try
            {
                ordenC.id_pedido = obj.id_pedido;
                ordenC.campo= obj.campo;
                ordenC.valor = obj.valor;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_POST_OUTBOUND_UPDATE_ORDER, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_pedido", SqlDbType.Int).Value = obj.id_pedido;
                    cmd.Parameters.Add("@campo", SqlDbType.NVarChar).Value = obj.campo;
                    cmd.Parameters.Add("@valor", SqlDbType.NVarChar).Value = obj.valor;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var Order = new OrderPickingBody();
                        Order.id_pedido = int.Parse(sqlReader["id"].ToString());
                        Order.id_almacen = sqlReader["id_almacen"].ToString();
                        Order.cliente = sqlReader["cliente"].ToString();
                        Order.orden = sqlReader["orden"].ToString();
                        Order.tipo = sqlReader["tipo"].ToString();
                        Order.urgencia = sqlReader["urgencia"].ToString();
                        Order.lineas = int.Parse(sqlReader["lineas"].ToString());
                        Order.cantidad_solicitada = decimal.Parse(sqlReader["cantidad_solicitada"].ToString());
                        Order.avance_picking = decimal.Parse(sqlReader["avance_picking"].ToString());
                        Order.envio = sqlReader["envio"].ToString();
                        Order.status = sqlReader["status"].ToString();


                        DateTime? fecha_despacho_objetivo = sqlReader.IsDBNull(sqlReader.GetOrdinal("fecha_despacho_objetivo")) ? null : (DateTime?)sqlReader.GetDateTime(sqlReader.GetOrdinal("fecha_despacho_objetivo"));
                        string fechaFormateada = fecha_despacho_objetivo.HasValue ? fecha_despacho_objetivo.Value.ToString(fechaFormatoGlobal) : string.Empty;
                        Order.fecha_despacho_objetivo = fechaFormateada;

                        DateTime? fecha_despacho_real = sqlReader.IsDBNull(sqlReader.GetOrdinal("fecha_despacho_real")) ? null : (DateTime?)sqlReader.GetDateTime(sqlReader.GetOrdinal("fecha_despacho_real"));
                        fechaFormateada = fecha_despacho_real.HasValue ? fecha_despacho_real.Value.ToString(fechaFormatoGlobal) : string.Empty;
                        Order.fecha_despacho_real = fechaFormateada;

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
        public List<OutboundTransactionBodyWeb> SP_OUTBOUND_WEB_GET_SHIPPING(OutboundTransactionRequestWeb obj, string cnx)
        {
            OutboundTransactionRequestWeb ordenC = new OutboundTransactionRequestWeb();
            List<OutboundTransactionBodyWeb> orderList = new List<OutboundTransactionBodyWeb>();
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
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_OUTBOUND_SHIPPING, conn))
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
                        var Order = new OutboundTransactionBodyWeb();
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


        //---

    }
}
