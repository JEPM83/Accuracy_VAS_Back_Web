using AccuracyData.ObjectosDA;
using AccuracyModel.Inventory;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyData.InventoryDA
{
    public class InventoryWebDA
    {
        static string fechaFormatoGlobal = "yyyy-MM-dd HH:mm:ss";
        public List<InventoryTransactionBodyWeb> SP_INVENTORY_WEB_GET_TRANSACTION_LOG(InventoryTransactionRequestWeb obj, string cnx)
        {
            InventoryTransactionRequestWeb ordenC = new InventoryTransactionRequestWeb();
            List<InventoryTransactionBodyWeb> orderList = new List<InventoryTransactionBodyWeb>();
            try
            {
                ordenC.id_almacen = obj.id_almacen;
                ordenC.numero_control= obj.numero_control;
                ordenC.id_ubicacion = obj.id_ubicacion;
                ordenC.id_empleado = obj.id_empleado;
                ordenC.fecha_inicial = obj.fecha_inicial;
                ordenC.fecha_final = obj.fecha_final;
                ordenC.numero_item = obj.numero_item;
                ordenC.tipo_transaccion = obj.tipo_transaccion;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_TRANSACTION_LOG, conn))
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
                        var Order = new InventoryTransactionBodyWeb();
                        Order.id = sqlReader["id"].ToString();
                        Order.tipo_transaccion = sqlReader["tipo_transaccion"].ToString();

                        DateTime? inicio_fecha = sqlReader.IsDBNull(sqlReader.GetOrdinal("inicio_fecha")) ? null : (DateTime?)sqlReader.GetDateTime(sqlReader.GetOrdinal("inicio_fecha"));
                        string fechaFormateada = inicio_fecha.HasValue ? inicio_fecha.Value.ToString(fechaFormatoGlobal) : string.Empty;
                        Order.inicio_fecha = fechaFormateada;

                        DateTime? fin_fecha = sqlReader.IsDBNull(sqlReader.GetOrdinal("fin_fecha")) ? null : (DateTime?)sqlReader.GetDateTime(sqlReader.GetOrdinal("fin_fecha"));
                        fechaFormateada = fin_fecha.HasValue ? fin_fecha.Value.ToString(fechaFormatoGlobal) : string.Empty;
                        Order.fin_fecha = fechaFormateada;

                        Order.id_empleado= sqlReader["id_empleado"].ToString();
                        Order.numero_control = sqlReader["numero_control"].ToString();
                        Order.id_almacen= sqlReader["id_almacen"].ToString();
                        Order.id_ubicacion = sqlReader["id_ubicacion"].ToString();
                        Order.numero_item = sqlReader["numero_item"].ToString();
                        Order.nombre_item = sqlReader["descripcion"].ToString();
                        Order.codigo_item = sqlReader["codigo_item"].ToString();
                        Order.familia = sqlReader["familia"].ToString();
                        Order.sub_familia = sqlReader["sub_familia"].ToString();
                        Order.talla = sqlReader["talla"].ToString();
                        Order.id_hu = sqlReader["id_hu"].ToString();
                        Order.numero_lote = sqlReader["numero_lote"].ToString();
                        Order.fecha_expiracion = sqlReader["fecha_expiracion"].ToString();
                        Order.cantidad_transaccion = decimal.Parse(sqlReader["cantidad_transaccion"].ToString());
                        Order.tipo_mov = sqlReader["tipo_mov"].ToString();
                        Order.atributo_generico_1 = sqlReader["atributo_generico_1"].ToString();
                        Order.atributo_generico_2 = sqlReader["atributo_generico_2"].ToString();
                        Order.id_hu_original = sqlReader["id_hu_original"].ToString();
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
        public List<ItemResponseWeb> SP_INVENTORY_WEB_GET_ITEM(InventoryRequestWeb obj, string cnx)
        {
            InventoryRequestWeb ordenC = new InventoryRequestWeb();
            List<ItemResponseWeb> orderList = new List<ItemResponseWeb>();
            try
            {
                ordenC.id_almacen = obj.id_almacen;
                ordenC.numero_item = obj.numero_item;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_ITEM, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@numero_item", SqlDbType.NVarChar).Value = obj.numero_item;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var Order = new ItemResponseWeb();
                        Order.id_almacen = sqlReader["id_almacen"].ToString();
                        Order.numero_item = sqlReader["numero_item"].ToString();
                        Order.descripcion = sqlReader["descripcion"].ToString();
                        Order.tipo_inventario = sqlReader["tipo_inventario"].ToString();
                        Order.unidad_medida = sqlReader["unidad_medida"].ToString();
                        Order.id_clase = sqlReader["id_clase"].ToString();
                        Order.upc = sqlReader["upc"].ToString();
                        Order.uom = sqlReader["uom"].ToString();
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
        public List<InventoryResponseWeb> SP_INVENTORY_WEB_GET_INVENTORY_ITEM(InventoryRequestWeb obj, string cnx)
        {
            InventoryRequestWeb ordenC = new InventoryRequestWeb();
            List<InventoryResponseWeb> orderList = new List<InventoryResponseWeb>();
            try
            {
                ordenC.id_almacen = obj.id_almacen;
                ordenC.numero_item = obj.numero_item;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_INVENTORY_ITEM, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@numero_item", SqlDbType.NVarChar).Value = obj.numero_item;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var Order = new InventoryResponseWeb();
                        Order.numero_item = sqlReader["numero_item"].ToString();
                        Order.numero_lote = sqlReader["numero_lote"].ToString();
                        Order.fecha_expiracion = sqlReader["fecha_expiracion"].ToString();
                        Order.id_almacen = sqlReader["id_almacen"].ToString();
                        Order.id_ubicacion = sqlReader["id_ubicacion"].ToString();
                        Order.id_hu = sqlReader["id_hu"].ToString();
                        Order.cantidad_actual = float.Parse(sqlReader["cantidad_actual"].ToString());
                        
                        DateTime? fecha_fifo = sqlReader.IsDBNull(sqlReader.GetOrdinal("fecha_fifo")) ? null : (DateTime?)sqlReader.GetDateTime(sqlReader.GetOrdinal("fecha_fifo"));
                        string fechaFormateada = fecha_fifo.HasValue ? fecha_fifo.Value.ToString(fechaFormatoGlobal) : string.Empty;
                        Order.fecha_fifo = fechaFormateada;

                        Order.cantidad_nodisponible = float.Parse(sqlReader["cantidad_nodisponible"].ToString());
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
        public List<InventoryResponseWeb> SP_INVENTORY_WEB_GET_INVENTORY_WAREHOUSE(WarehouseRequestWeb obj, string cnx)
        {
            InventoryRequestWeb ordenC = new InventoryRequestWeb();
            List<InventoryResponseWeb> orderList = new List<InventoryResponseWeb>();
            try
            {
                ordenC.id_almacen = obj.id_almacen;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_INVENTORY_WAREHOUSE, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = obj.id_almacen;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var Order = new InventoryResponseWeb();
                        Order.numero_item = sqlReader["numero_item"].ToString();
                        Order.numero_lote = sqlReader["numero_lote"].ToString();
                        Order.fecha_expiracion = sqlReader["fecha_expiracion"].ToString();
                        Order.id_almacen = sqlReader["id_almacen"].ToString();
                        Order.id_ubicacion = sqlReader["id_ubicacion"].ToString();
                        Order.id_hu = sqlReader["id_hu"].ToString();
                        Order.cantidad_actual = float.Parse(sqlReader["cantidad_actual"].ToString());

                        DateTime? fecha_fifo = sqlReader.IsDBNull(sqlReader.GetOrdinal("fecha_fifo")) ? null : (DateTime?)sqlReader.GetDateTime(sqlReader.GetOrdinal("fecha_fifo"));
                        string fechaFormateada = fecha_fifo.HasValue ? fecha_fifo.Value.ToString(fechaFormatoGlobal) : string.Empty;
                        Order.fecha_fifo = fechaFormateada;

                        Order.cantidad_nodisponible = float.Parse(sqlReader["cantidad_nodisponible"].ToString());
                        Order.atributo_generico_1 = sqlReader["atributo_generico_1"].ToString();
                        Order.atributo_generico_2 = sqlReader["atributo_generico_2"].ToString();
                        Order.nombre_item = sqlReader["descripcion"].ToString();
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
        public List<KardexBodyWeb> SP_INVENTORY_WEB_GET_KARDEX(KardexRequestWeb obj, string cnx)
        {
            KardexRequestWeb ordenC = new KardexRequestWeb();
            List<KardexBodyWeb> orderList = new List<KardexBodyWeb>();
            try
            {
                ordenC.id_almacen = obj.id_almacen;
                ordenC.fecha_inicial = obj.fecha_inicial;
                ordenC.fecha_final = obj.fecha_final;
                ordenC.numero_item = obj.numero_item;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_INVENTORY_KARDEX, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@fecha_inicial", SqlDbType.NVarChar).Value = obj.fecha_inicial;
                    cmd.Parameters.Add("@fecha_final", SqlDbType.NVarChar).Value = obj.fecha_final;
                    cmd.Parameters.Add("@numero_item", SqlDbType.NVarChar).Value = obj.numero_item;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var Order = new KardexBodyWeb();
                        Order.TotalForecasting = float.Parse(sqlReader["TotalForecasting"].ToString());
                        Order.Articulo = sqlReader["Articulo"].ToString();
                        Order.Descripcion = sqlReader["Descripcion"].ToString();
                        Order.Fecha = DateTime.Parse(sqlReader["Fecha"].ToString());
                        Order.Movimiento = sqlReader["Movimiento"].ToString();
                        Order.Guia = sqlReader["Guia"].ToString();
                        Order.Documento = sqlReader["Documento"].ToString();
                        Order.Serie = sqlReader["Serie"].ToString();
                        Order.Numero = float.Parse(sqlReader["Numero"].ToString());
                        Order.Cliente_Proveedor = sqlReader["Cliente_Proveedor"].ToString();
                        Order.Observacion = sqlReader["Observacion"].ToString();
                        Order.Ubicacion = sqlReader["Ubicacion"].ToString();
                        Order.CantidadE = float.Parse(sqlReader["CantidadE"].ToString());
                        Order.PrecioE = float.Parse(sqlReader["PrecioE"].ToString());
                        Order.TotalE = float.Parse(sqlReader["TotalE"].ToString());
                        Order.CantidadS = float.Parse(sqlReader["CantidadS"].ToString());
                        Order.PrecioS = float.Parse(sqlReader["PrecioS"].ToString());
                        Order.TotalS = float.Parse(sqlReader["TotalS"].ToString());
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
        public List<CurvaResponsetWeb> SP_INVENTORY_WEB_GET_CURVA(CurvaRequestWeb obj, string cnx)
        {
            CurvaRequestWeb ordenC = new CurvaRequestWeb();
            List<CurvaResponsetWeb> orderList = new List<CurvaResponsetWeb>();
            try
            {
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_INVENTORY_CURVA, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@numero_item", SqlDbType.NVarChar).Value = obj.numero_item;
                    cmd.Parameters.Add("@curva", SqlDbType.NVarChar).Value = obj.curva;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var Curva = new CurvaResponsetWeb();
                        Curva.curva = sqlReader["curva"].ToString();
                        Curva.talla = sqlReader["talla"].ToString();
                        Curva.cantidad = int.Parse(sqlReader["cantidad"].ToString());
                        orderList.Add(Curva);
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
    }
}
