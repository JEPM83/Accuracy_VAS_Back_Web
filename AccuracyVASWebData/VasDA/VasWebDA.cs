﻿using AccuracyData.ObjectosDA;
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
    }
}

