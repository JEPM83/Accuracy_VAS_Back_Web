using AccuracyData.ObjectosDA;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccuracyModel.Vas;

namespace AccuracyData.VasDA
{
    public class VasWebDA
    {
        public List<LineResponse> GET_PRODUCTION_LINE_BY_TERMINAL(LineRequest model,string cnx)
        {
            var plListDetail = new List<LineResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_GET_PRODUCTION_LINE_BY_TERMINAL, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@direccion", SqlDbType.NVarChar).Value = model.direccion;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new LineResponse();
                        plDetail.linea_produccion = sqlReader["linea_produccion"].ToString();
                        plDetail.estado = int.Parse(sqlReader["estado"].ToString());
                        plDetail.mensaje = sqlReader["mensaje"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<OrderPedidoResponse> GET_ORDER_VAS(OrderPedidoRequest model, string cnx)
        {
            var plListDetail = new List<OrderPedidoResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_GET_ORDER_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new OrderPedidoResponse();
                        plDetail.cliente = sqlReader["cliente"].ToString();
                        plDetail.tienda = sqlReader["tienda"].ToString();
                        plDetail.numero_pedido = sqlReader["numero_pedido"].ToString();
                        plDetail.avance_vas = float.Parse(sqlReader["avance_vas"].ToString());
                        plDetail.lineas_produccion = int.Parse(sqlReader["lineas_produccion"].ToString());
                        plDetail.id_hu = sqlReader["id_hu"].ToString();
                        plDetail.categoria_inventario = sqlReader["categoria_inventario"].ToString();
                        plDetail.color_fondo = sqlReader["color_fondo"].ToString();
                        plDetail.medida = sqlReader["medida"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<NotifyOrderResponse> POST_NOTIFY_ORDER_VAS(NotifyOrderRequest model, string cnx)
        {
            var plListDetail = new List<NotifyOrderResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_POST_NOTIFY_ORDER_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@linea_produccion", SqlDbType.NVarChar).Value = model.linea_produccion;
                    cmd.Parameters.Add("@id_hu", SqlDbType.NVarChar).Value = model.id_hu;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = model.usuario;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new NotifyOrderResponse();
                        plDetail.type = sqlReader["estado"].ToString();
                        plDetail.message = sqlReader["mensaje"].ToString();
                        plDetail.tittle = sqlReader["titulo"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<OrderPedidoDetailPickingResponse> GET_ORDER_DETAIL_PICKING_VAS(OrderPedidoDetailPickingRequest model, string cnx)
        {
            var plListDetail = new List<OrderPedidoDetailPickingResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_GET_ORDER_DETAIL_PICKING_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@id_hu", SqlDbType.NVarChar).Value = model.id_hu;
                    cmd.Parameters.Add("@numero_item", SqlDbType.NVarChar).Value = model.numero_item;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new OrderPedidoDetailPickingResponse();
                        plDetail.numero_item = sqlReader["numero_item"].ToString();
                        plDetail.descripcion = sqlReader["descripcion"].ToString();
                        plDetail.unidad_medida = sqlReader["unidad_medida"].ToString();
                        plDetail.cantidad_picking = float.Parse(sqlReader["cantidad_picking"].ToString());
                        plDetail.categoria_inventario = sqlReader["categoria_inventario"].ToString();
                        plDetail.subcategoria_inventario = sqlReader["subcategoria_inventario"].ToString();
                        plDetail.atributo_generico_1 = sqlReader["atributo_generico_1"].ToString();
                        plDetail.atributo_generico_2 = sqlReader["atributo_generico_2"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<TaskResponse> GET_ORDER_DETAIL_TASK_VAS(TaskRequest model, string cnx)
        {
            var plListDetail = new List<TaskResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_GET_ORDER_DETAIL_TASK_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@id_hu", SqlDbType.NVarChar).Value = model.id_hu;
                    cmd.Parameters.Add("@numero_item", SqlDbType.NVarChar).Value = model.numero_item;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new TaskResponse();
                        
                        //plDetail.numero_pedido = sqlReader["numero_pedido"].ToString();
                        plDetail.cliente = sqlReader["cliente"].ToString();
                        plDetail.categoria_inventario = sqlReader["categoria_inventario"].ToString();
                        plDetail.id_agrupador = int.Parse(sqlReader["id_agrupador"].ToString());
                        plDetail.descripcion_agrupador = sqlReader["descripcion_agrupador"].ToString();
                        plDetail.id_tarea =  int.Parse(sqlReader["id_tarea"].ToString());
                        plDetail.secuencia = int.Parse(sqlReader["secuencia"].ToString());
                        plDetail.descripcion_tarea = sqlReader["descripcion_tarea"].ToString();
                        plDetail.comentario = sqlReader["comentario"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<InicioTareaResponse> POST_START_TASK_VAS(InicioTareaRequest model, string cnx)
        {
            var plListDetail = new List<InicioTareaResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_POST_START_TASK_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@linea_produccion", SqlDbType.NVarChar).Value = model.linea_produccion;
                    cmd.Parameters.Add("@id_hu", SqlDbType.NVarChar).Value = model.id_hu;
                    cmd.Parameters.Add("@numero_item", SqlDbType.NVarChar).Value = model.numero_item;
                    cmd.Parameters.Add("@id_tarea", SqlDbType.Int).Value = model.id_tarea;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = model.usuario;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new InicioTareaResponse();
                        plDetail.type = sqlReader["estado"].ToString();
                        plDetail.message = sqlReader["mensaje"].ToString();
                        plDetail.tittle = sqlReader["titulo"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<PackingdetailResponse> GET_HU_DETAIL_BY_ORDER_VAS(PackingdetailRequest model, string cnx)
        {
            var plListDetail = new List<PackingdetailResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_GET_HU_DETAIL_BY_ORDER_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@linea_produccion", SqlDbType.NVarChar).Value = model.linea_produccion;
                    cmd.Parameters.Add("@id_hu", SqlDbType.NVarChar).Value = model.id_hu;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new PackingdetailResponse();
                        plDetail.lpn = sqlReader["lpn"].ToString();
                        plDetail.numero_item = sqlReader["numero_item"].ToString();
                        plDetail.descripcion = sqlReader["descripcion"].ToString();
                        plDetail.cantidad = float.Parse(sqlReader["cantidad"].ToString());
                        plDetail.unidad_medida = sqlReader["unidad_medida"].ToString();
                        plDetail.talla_b2b = sqlReader["talla_b2b"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<LPNvalidaResponse> GET_LPN_VALIDATE_VAS(LPNvalidateRequest model, string cnx)
        {
            var plListDetail = new List<LPNvalidaResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_GET_LPN_VALIDATE_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@lpn", SqlDbType.NVarChar).Value = model.lpn;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new LPNvalidaResponse();
                        plDetail.type = sqlReader["estado"].ToString();
                        plDetail.message = sqlReader["mensaje"].ToString();
                        plDetail.tittle = sqlReader["titulo"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<LPNSKUResponse> POST_LPN_SKU_VAS(LPNSKURequest model, string cnx)
        {
            var plListDetail = new List<LPNSKUResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_POST_LPN_SKU_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@linea_produccion", SqlDbType.NVarChar).Value = model.linea_produccion;
                    cmd.Parameters.Add("@id_hu", SqlDbType.NVarChar).Value = model.id_hu;
                    cmd.Parameters.Add("@lpn", SqlDbType.NVarChar).Value = model.lpn;
                    cmd.Parameters.Add("@numero_item", SqlDbType.NVarChar).Value = model.numero_item;
                    cmd.Parameters.Add("@cantidad", SqlDbType.Float).Value = model.cantidad;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = model.usuario;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new LPNSKUResponse();
                        plDetail.type = sqlReader["estado"].ToString();
                        plDetail.message = sqlReader["mensaje"].ToString();
                        plDetail.tittle = sqlReader["titulo"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<DeleteVasResponse> POST_DELETE_VAS(DeleteVasRequest model, string cnx)
        {
            var plListDetail = new List<DeleteVasResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_POST_DELETE_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@linea_produccion", SqlDbType.NVarChar).Value = model.linea_produccion;
                    cmd.Parameters.Add("@id_hu", SqlDbType.NVarChar).Value = model.id_hu;
                    cmd.Parameters.Add("@lpn", SqlDbType.NVarChar).Value = model.lpn;
                    cmd.Parameters.Add("@numero_item", SqlDbType.NVarChar).Value = model.numero_item;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = model.usuario;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new DeleteVasResponse();
                        plDetail.type = sqlReader["estado"].ToString();
                        plDetail.message = sqlReader["mensaje"].ToString();
                        plDetail.tittle = sqlReader["titulo"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<FinTareaResponse> POST_END_TASK_VAS(FinTareaRequest model, string cnx)
        {
            var plListDetail = new List<FinTareaResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_POST_END_TASK_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@linea_produccion", SqlDbType.NVarChar).Value = model.linea_produccion;
                    cmd.Parameters.Add("@id_hu", SqlDbType.NVarChar).Value = model.id_hu;
                    cmd.Parameters.Add("@numero_item", SqlDbType.NVarChar).Value = model.numero_item;
                    cmd.Parameters.Add("@id_tarea", SqlDbType.Int).Value = model.id_tarea;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = model.usuario;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new FinTareaResponse();
                        plDetail.type = sqlReader["estado"].ToString();
                        plDetail.message = sqlReader["mensaje"].ToString();
                        plDetail.tittle = sqlReader["titulo"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<InicioIncidenciaResponse> POST_START_INCIDENCE_VAS(InicioIncidenciaRequest model, string cnx)
        {
            var plListDetail = new List<InicioIncidenciaResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_POST_START_INCIDENCE_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@linea_produccion", SqlDbType.NVarChar).Value = model.linea_produccion;
                    cmd.Parameters.Add("@id_hu", SqlDbType.NVarChar).Value = model.id_hu;
                    cmd.Parameters.Add("@id_razon", SqlDbType.Int).Value = model.id_razon;
                    cmd.Parameters.Add("@descripcion_razon", SqlDbType.NVarChar).Value = model.descripcion_razon;
                    cmd.Parameters.Add("@observacion", SqlDbType.NVarChar).Value = model.observacion;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = model.usuario;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new InicioIncidenciaResponse();
                        plDetail.type = sqlReader["estado"].ToString();
                        plDetail.message = sqlReader["mensaje"].ToString();
                        plDetail.tittle = sqlReader["titulo"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<FinIncidenciaResponse> POST_END_INCIDENCE_VAS(FinIncidenciaRequest model, string cnx)
        {
            var plListDetail = new List<FinIncidenciaResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_POST_END_INCIDENCE_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@linea_produccion", SqlDbType.NVarChar).Value = model.linea_produccion;
                    cmd.Parameters.Add("@id_hu", SqlDbType.NVarChar).Value = model.id_hu;
                    cmd.Parameters.Add("@id_razon", SqlDbType.Int).Value = model.id_razon;
                    cmd.Parameters.Add("@descripcion_razon", SqlDbType.NVarChar).Value = model.descripcion_razon;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = model.usuario;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new FinIncidenciaResponse();
                        plDetail.type = sqlReader["estado"].ToString();
                        plDetail.message = sqlReader["mensaje"].ToString();
                        plDetail.tittle = sqlReader["titulo"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<ListaNotificacionResponse> GET_LIST_NOTIFY_VAS(ListaNotificacionRequest model, string cnx)
        {
            var plListDetail = new List<ListaNotificacionResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_GET_LIST_NOTIFY_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@proceso", SqlDbType.NVarChar).Value = model.proceso;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new ListaNotificacionResponse();
                        plDetail.id_notificacion = int.Parse(sqlReader["id_notificacion"].ToString());
                        plDetail.proceso = sqlReader["proceso"].ToString();
                        plDetail.linea_produccion = sqlReader["linea_produccion"].ToString();
                        plDetail.usuario = sqlReader["usuario"].ToString();
                        plDetail.id_razon = int.Parse( sqlReader["id_razon"].ToString());
                        plDetail.descripcion_razon = sqlReader["descripcion_razon"].ToString();
                        plDetail.observacion = sqlReader["observacion"].ToString();
                        plDetail.minutos_atras_reportado = int.Parse(sqlReader["minutos_atras_reportado"].ToString());
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<ActualizacionNotificacionResponse> POST_UPDATE_NOTIFY_VAS(ActualizacionNotificacionRequest model, string cnx)
        {
            var plListDetail = new List<ActualizacionNotificacionResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_POST_UPDATE_NOTIFY_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@proceso", SqlDbType.NVarChar).Value = model.proceso;
                    cmd.Parameters.Add("@id_notificacion", SqlDbType.NVarChar).Value = model.id_notificacion;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = model.usuario;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new ActualizacionNotificacionResponse();
                        plDetail.type = sqlReader["estado"].ToString();
                        plDetail.message = sqlReader["mensaje"].ToString();
                        plDetail.tittle = sqlReader["titulo"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<PanelLineaResponse> GET_PANEL_LINEA_PRODUCCION_VAS(PanelLineaRequest model, string cnx)
        {
            var plListDetail = new List<PanelLineaResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_GET_PANEL_LINEA_PRODUCCION_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new PanelLineaResponse();
                        plDetail.linea_produccion = sqlReader["linea_produccion"].ToString();
                        plDetail.nombres = sqlReader["nombres"].ToString();
                        plDetail.numero_pedido = sqlReader["numero_pedido"].ToString();
                        plDetail.cantidad_vas = float.Parse(sqlReader["cantidad_vas"].ToString());
                        plDetail.estado = int.Parse(sqlReader["estado"].ToString());
                        plDetail.semaforo = sqlReader["semaforo"].ToString();
                        plDetail.tiempo = sqlReader["tiempo"].ToString();
                        plDetail.fila = int.Parse(sqlReader["fila"].ToString());
                        plDetail.columna = int.Parse(sqlReader["columna"].ToString());
                        plDetail.grupo = int.Parse(sqlReader["grupo"].ToString());
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<PanelOrdenResponse> GET_PANEL_ORDER_PRODUCCION_VAS(PanelOrdenRequest model, string cnx)
        {
            var plListDetail = new List<PanelOrdenResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_GET_PANEL_ORDER_PRODUCCION_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new PanelOrdenResponse();
                        plDetail.numero_pedido = sqlReader["numero_pedido"].ToString();
                        plDetail.cantidad_picada = float.Parse(sqlReader["cantidad_picada"].ToString());
                        plDetail.cantidad_vas = float.Parse(sqlReader["cantidad_vas"].ToString());
                        plDetail.estado = int.Parse(sqlReader["estado"].ToString());
                        plDetail.color_barra = sqlReader["color_barra"].ToString();
                        plDetail.total_linea_produccion = int.Parse(sqlReader["total_linea_produccion"].ToString());
                        plDetail.total_usuario = int.Parse(sqlReader["total_usuario"].ToString());
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        /*Vistas supervisor*/
        public List<OrdenesVasResponse> GET_LIST_ORDER_PRODUCCION_VAS(OrdenesVasRequest model, string cnx)
        {
            var plListDetail = new List<OrdenesVasResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_GET_LIST_ORDER_PRODUCCION_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@fecha_inicial_despacho", SqlDbType.Date).Value = model.fecha_inicial_despacho;
                    cmd.Parameters.Add("@fecha_final_despacho", SqlDbType.Date).Value = model.fecha_final_despacho;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new OrdenesVasResponse();
                        plDetail.id_pedido = int.Parse(sqlReader["id_pedido"].ToString());
                        plDetail.proveedor = sqlReader["proveedor"].ToString();
                        plDetail.numero_pedido = sqlReader["numero_pedido"].ToString();
                        plDetail.id_destino = sqlReader["id_destino"].ToString();
                        plDetail.destino = sqlReader["destino"].ToString();
                        plDetail.familia_producto = sqlReader["familia_producto"].ToString();
                        plDetail.cantidad = float.Parse(sqlReader["cantidad"].ToString());
                        plDetail.avance = float.Parse(sqlReader["avance"].ToString());
                        plDetail.fecha_despacho = sqlReader["fecha_despacho"].ToString();
                        plDetail.estado = sqlReader["estado"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<UsuariosVasResponse> GET_LIST_STATE_USERS_VAS(UsuariosVasRequest model, string cnx)
        {
            var plListDetail = new List<UsuariosVasResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_GET_LIST_STATE_USERS_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@fecha_inicial_despacho", SqlDbType.Date).Value = model.fecha_inicial_despacho;
                    cmd.Parameters.Add("@fecha_final_despacho", SqlDbType.Date).Value = model.fecha_final_despacho;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new UsuariosVasResponse();
                        plDetail.colaborador = sqlReader["colaborador"].ToString();
                        plDetail.linea_produccion = sqlReader["linea_produccion"].ToString();
                        plDetail.estado = sqlReader["estado"].ToString();
                        plDetail.numero_pedido = sqlReader["numero_pedido"].ToString();
                        plDetail.destino = sqlReader["destino"].ToString();
                        plDetail.categoria_inventario = sqlReader["categoria_inventario"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<B2BVasResponse> GET_LIST_ORDER_B2B_VAS(B2BVasRequest model, string cnx)
        {
            var plListDetail = new List<B2BVasResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_GET_LIST_ORDER_B2B_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@fecha_inicial_despacho", SqlDbType.Date).Value = model.fecha_inicial_despacho;
                    cmd.Parameters.Add("@fecha_final_despacho", SqlDbType.Date).Value = model.fecha_final_despacho;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new B2BVasResponse();
                        plDetail.o_r = sqlReader["o_r"].ToString();
                        plDetail.o_c = sqlReader["o_c"].ToString();
                        plDetail.id_destino = sqlReader["id_destino"].ToString();
                        plDetail.nombre_destino = sqlReader["nombre_destino"].ToString();
                        plDetail.destino_cod = sqlReader["destino_cod"].ToString();
                        plDetail.destino_des = sqlReader["destino_des"].ToString();
                        plDetail.sku = sqlReader["sku"].ToString();
                        plDetail.item = sqlReader["item"].ToString();
                        plDetail.talla = sqlReader["talla"].ToString();
                        plDetail.cantidad = sqlReader["cantidad"].ToString();
                        plDetail.ean13 = sqlReader["ean13"].ToString();
                        plDetail.precio = sqlReader["precio"].ToString();
                        plDetail.lpn = sqlReader["lpn"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<SendB2BVas_atributos> GET_LIST_ORDER_B2B_VAS_V2(SendB2BVas_baseRequest model, string cnx)
        {
            var plListDetail = new List<SendB2BVas_atributos>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_GET_LIST_ORDER_B2B_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@fecha_inicial_despacho", SqlDbType.Date).Value = model.fecha_inicial_despacho;
                    cmd.Parameters.Add("@fecha_final_despacho", SqlDbType.Date).Value = model.fecha_final_despacho;
                    cmd.Parameters.Add("@id_pedido", SqlDbType.Int).Value = model.id_pedido;
                    cmd.Parameters.Add("@id_destino", SqlDbType.NVarChar).Value = model.id_destino;
                    cmd.Parameters.Add("@cita", SqlDbType.NVarChar).Value = model.cita;
                    cmd.Parameters.Add("@fecha_factura", SqlDbType.NVarChar).Value = model.fecha_factura;
                    cmd.Parameters.Add("@factura", SqlDbType.NVarChar).Value = model.factura;
                    cmd.Parameters.Add("@lote", SqlDbType.NVarChar).Value = model.lote;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        if (model.id_destino == "D003" || model.id_destino == "D004")//RIPLEY
                    {
                        var plDetail = new SendB2BVas_Modelo1Detalle();
                        plDetail.numero_de_cita = sqlReader["numero_de_cita"].ToString();
                        plDetail.numero_de_oc = sqlReader["numero_de_oc"].ToString();
                        plDetail.ruc = sqlReader["ruc"].ToString();
                        plDetail.fecha = sqlReader["fecha"].ToString();
                        plDetail.documento = sqlReader["documento"].ToString();
                        plDetail.contenedor = sqlReader["contenedor"].ToString();

                        plDetail.correlativo = int.Parse(sqlReader["correlativo"].ToString());
                        plDetail.articulo = sqlReader["articulo"].ToString();
                        plDetail.cantidad = float.Parse(sqlReader["cantidad"].ToString());
                        plDetail.na = sqlReader["na"].ToString();
                        plDetail.cod_sucursal = sqlReader["cod_sucursal"].ToString();
                        plDetail.costo_unitario = !sqlReader.IsDBNull(sqlReader.GetOrdinal("costo_unitario")) ? float.Parse(sqlReader["costo_unitario"].ToString()) : 0.0f; ;
                        plDetail.columna = int.Parse(sqlReader["columna"].ToString());
                        plDetail.fila = int.Parse(sqlReader["fila"].ToString()); 
                        plDetail.salto = int.Parse(sqlReader["salto"].ToString());
                        plDetail.titulo = sqlReader["titulo"].ToString();
                        plDetail.hoja = sqlReader["hoja"].ToString();
                        plDetail.color_fondo_titulo_grilla = sqlReader["color_fondo_titulo_grilla"].ToString();
                        plDetail.color_letra_titulo_grilla = sqlReader["color_letra_titulo_grilla"].ToString();
                        plDetail.nombre_archivo = sqlReader["nombre_archivo"].ToString();
                        plDetail.extension_archivo = sqlReader["extension_archivo"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    else if (model.id_destino == "D001" || model.id_destino == "D002")//SAGA
                    {
                        var plDetail = new SendB2BVas_Modelo2Detalle();
                        plDetail.nro_lote = sqlReader["nro_lote"].ToString();

                        plDetail.negocio = sqlReader["negocio"].ToString();
                        plDetail.orden_de_compra = sqlReader["orden_de_compra"].ToString();
                        plDetail.upc_ean = sqlReader["upc_ean"].ToString();
                        plDetail.descripcion = sqlReader["descripcion"].ToString();
                        plDetail.caducidad = sqlReader["caducidad"].ToString();
                        plDetail.cantidad = float.Parse(sqlReader["cantidad"].ToString());
                        plDetail.cantidad_a_enviar = float.Parse(sqlReader["cantidad_a_enviar"].ToString());
                        plDetail.tienda_destino = sqlReader["tienda_destino"].ToString();
                        plDetail.nombre_tienda = sqlReader["nombre_tienda"].ToString();
                        plDetail.tipo_embalaje = sqlReader["tipo_embalaje"].ToString();
                        plDetail.lpn = sqlReader["lpn"].ToString();
                            plDetail.columna = int.Parse(sqlReader["columna"].ToString());
                            plDetail.fila = int.Parse(sqlReader["fila"].ToString());
                            plDetail.salto = int.Parse(sqlReader["salto"].ToString());
                        plDetail.titulo = sqlReader["titulo"].ToString();
                        plDetail.hoja = sqlReader["hoja"].ToString();
                        plDetail.color_fondo_titulo_grilla = sqlReader["color_fondo_titulo_grilla"].ToString();
                        plDetail.color_letra_titulo_grilla = sqlReader["color_letra_titulo_grilla"].ToString();
                        plDetail.nombre_archivo = sqlReader["nombre_archivo"].ToString();
                        plDetail.extension_archivo = sqlReader["extension_archivo"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    else if (model.id_destino == "D005" || model.id_destino == "D006")//TIENDAS PERUANAS
                    {
                        var plDetail = new SendB2BVas_Modelo3Detalle();
                        plDetail.codigo_proveedor = sqlReader["codigo_proveedor"].ToString();
                        plDetail.sucursal_destino = sqlReader["sucursal_destino"].ToString();
                        plDetail.n_de_oc = sqlReader["n_de_oc"].ToString();
                        plDetail.lpn = sqlReader["lpn"].ToString();
                        plDetail.codigo_producto = sqlReader["codigo_producto"].ToString();
                        plDetail.cantidad_und = float.Parse(sqlReader["cantidad_und"].ToString());
                            plDetail.columna = int.Parse(sqlReader["columna"].ToString());
                            plDetail.fila = int.Parse(sqlReader["fila"].ToString());
                            plDetail.salto = int.Parse(sqlReader["salto"].ToString());
                        plDetail.titulo = sqlReader["titulo"].ToString();
                        plDetail.hoja = sqlReader["hoja"].ToString();
                        plDetail.color_fondo_titulo_grilla = sqlReader["color_fondo_titulo_grilla"].ToString();
                        plDetail.color_letra_titulo_grilla = sqlReader["color_letra_titulo_grilla"].ToString();
                        plDetail.nombre_archivo = sqlReader["nombre_archivo"].ToString();
                        plDetail.extension_archivo = sqlReader["extension_archivo"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    else {//OTROS   
            
                        var plDetail = new SendB2BVas_Modelo4Detalle();
                        plDetail.o_r = sqlReader["o_r"].ToString();
                        plDetail.o_c = sqlReader["o_c"].ToString();
                        plDetail.id_destino = sqlReader["id_destino"].ToString();
                        plDetail.nombre_destino = sqlReader["nombre_destino"].ToString();
                        plDetail.destino_cod = sqlReader["destino_cod"].ToString();
                        plDetail.destino_des = sqlReader["destino_des"].ToString();
                        plDetail.sku = sqlReader["sku"].ToString();
                        plDetail.item = sqlReader["item"].ToString();
                        plDetail.talla = sqlReader["talla"].ToString();
                        plDetail.cantidad = float.Parse(sqlReader["cantidad"].ToString());
                        plDetail.ean13 = sqlReader["ean13"].ToString();
                        plDetail.precio = sqlReader["precio"].ToString();
                        plDetail.lpn = sqlReader["lpn"].ToString();
                            plDetail.columna = int.Parse(sqlReader["columna"].ToString());
                            plDetail.fila = int.Parse(sqlReader["fila"].ToString());
                            plDetail.salto = int.Parse(sqlReader["salto"].ToString());
                        plDetail.titulo = sqlReader["titulo"].ToString();
                        plDetail.hoja = sqlReader["hoja"].ToString();
                        plDetail.color_fondo_titulo_grilla = sqlReader["color_fondo_titulo_grilla"].ToString();
                        plDetail.color_letra_titulo_grilla = sqlReader["color_letra_titulo_grilla"].ToString();
                        plDetail.nombre_archivo = sqlReader["nombre_archivo"].ToString();
                        plDetail.extension_archivo = sqlReader["extension_archivo"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                        }
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<UpdateStateResponse> POST_UPDATE_STATE_DELIVERY_VAS(UpdateStateRequest model, string cnx)
        {
            var plListDetail = new List<UpdateStateResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_POST_UPDATE_STATE_DELIVERY_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@estado", SqlDbType.NVarChar).Value = model.estado;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = model.usuario;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new UpdateStateResponse();
                        plDetail.type = sqlReader["estado"].ToString();
                        plDetail.message = sqlReader["mensaje"].ToString();
                        plDetail.tittle = sqlReader["titulo"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        /*Impresion*/
        public List<PrinterLpnResponse> POST_PRINTER_VAS(PrinteLpnRequest model, string cnx)
        {
            var plListDetail = new List<PrinterLpnResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_POST_PRINTER_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@id_printer", SqlDbType.Int).Value = model.id_printer;
                    cmd.Parameters.Add("@modelo", SqlDbType.NVarChar).Value = model.modelo;
                    cmd.Parameters.Add("@numero_pedido", SqlDbType.NVarChar).Value = model.numero_pedido;
                    cmd.Parameters.Add("@id_destino", SqlDbType.NVarChar).Value = model.id_destino;
                    cmd.Parameters.Add("@linea", SqlDbType.NVarChar).Value = model.linea;
                    cmd.Parameters.Add("@lote", SqlDbType.NVarChar).Value = model.lote;
                    cmd.Parameters.Add("@cita", SqlDbType.NVarChar).Value = model.cita;
                    cmd.Parameters.Add("@factura", SqlDbType.NVarChar).Value = model.factura;
                    cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = model.cantidad;
                    cmd.Parameters.Add("@usuario_creacion", SqlDbType.NVarChar).Value = model.usuario_creacion;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new PrinterLpnResponse();
                        plDetail.type = sqlReader["STATE"].ToString();
                        plDetail.message = sqlReader["MESSAGE"].ToString();
                        plDetail.tittle = sqlReader["ErrorMessage"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<DestinoResponse> GET_LIST_DESTINITY(DestinoRequest model, string cnx)
        {
            var plListDetail = new List<DestinoResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_GET_LIST_DESTINITY, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@id_cliente", SqlDbType.NVarChar).Value = model.id_cliente;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = model.usuario;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new DestinoResponse();
                        plDetail.id_destino = sqlReader["id_destino"].ToString();
                        plDetail.nombre_destino = sqlReader["nombre_destino"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<ImprimirLpnResponse> POST_PRINTER_LPN_VAS(ImprimirLpnRequest model, string cnx)
        {
            var plListDetail = new List<ImprimirLpnResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_POST_PRINTER_LPN_VAS, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;      
                    cmd.Parameters.Add("@id_cliente", SqlDbType.NVarChar).Value = model.id_cliente;
                    cmd.Parameters.Add("@id_destino", SqlDbType.NVarChar).Value = model.id_destino;
                    cmd.Parameters.Add("@id_printer", SqlDbType.Int).Value = model.id_printer;
                    cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = model.cantidad;
                    cmd.Parameters.Add("@usuario_creacion", SqlDbType.NVarChar).Value = model.usuario_creacion;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new ImprimirLpnResponse();
                        plDetail.type = sqlReader["STATE"].ToString();
                        plDetail.message = sqlReader["MESSAGE"].ToString();
                        plDetail.tittle = sqlReader["ErrorMessage"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
        public List<CorrelativoLpnResponse> GET_LPN_CORRELATIVE_DESTINITY(CorrelativoLpnRequest model, string cnx)
        {
            var plListDetail = new List<CorrelativoLpnResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.VAS_GET_LPN_CORRELATIVE_DESTINITY, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = model.id_almacen;
                    cmd.Parameters.Add("@id_cliente", SqlDbType.NVarChar).Value = model.id_cliente;
                    cmd.Parameters.Add("@id_destino", SqlDbType.NVarChar).Value = model.id_destino;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new CorrelativoLpnResponse();
                        plDetail.prefijo = sqlReader["prefijo"].ToString();
                        plDetail.correlativo = sqlReader["correlativo"].ToString();
                        plListDetail.Add(plDetail);
                        plDetail = null;
                    }
                    conn.Close();
                    return plListDetail;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                plListDetail = null;
            }
        }
    }
}

