using AccuracyData.ObjectosDA;
using AccuracyModel.Forecast;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace AccuracyData.ForecastDA
{
    public class ForecastWebDA
    {
        public List<ForecasHeadtBodyWeb> SP_FORECAST_WEB_GET_HEAD(ForecastHeadRequestWeb obj, string cnx)
        {
            ForecastHeadRequestWeb ordenC = new ForecastHeadRequestWeb();
            List<ForecasHeadtBodyWeb> orderList = new List<ForecasHeadtBodyWeb>();
            try
            {
                ordenC.id_almacen = obj.id_almacen;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_FORECAST_HEAD, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.VarChar).Value = obj.id_almacen;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var Order = new ForecasHeadtBodyWeb();
                        Order.value = int.Parse(sqlReader["value"].ToString()); ;
                        Order.display = sqlReader["display"].ToString();
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
        public List<ForecastDetailBodyWeb> SP_FORECAST_WEB_GET_DETAIL(ForecastDetailRequestWeb obj, string cnx)
        {
            ForecastDetailRequestWeb ordenC = new ForecastDetailRequestWeb();
            List<ForecastDetailBodyWeb> orderList = new List<ForecastDetailBodyWeb>();
            try
            {
                ordenC.id_almacen = obj.id_almacen;
                ordenC.forecast = obj.forecast;

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_FORECAST_DETAIL, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@forecast", SqlDbType.NVarChar).Value = obj.forecast;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        var Order = new ForecastDetailBodyWeb();
                        Order.alerta = sqlReader["alerta"].ToString();
                        Order.forecast = sqlReader["forecast"].ToString();
                        Order.numero_item = sqlReader["numero_item"].ToString();
                        Order.descripcion_item = sqlReader["descripcion_item"].ToString();
                        Order.categoria_inventario = sqlReader["categoria_inventario"].ToString();
                        Order.subcategoria_inventario = sqlReader["subcategoria_inventario"].ToString();
                        Order.atributo_01 = sqlReader["atributo_01"].ToString();
                        Order.cantidad = float.Parse(sqlReader["cantidad"].ToString());
                        Order.cantidad_recibir = float.Parse(sqlReader["cantidad_recibir"].ToString());
                        Order.usuario_creacion = sqlReader["usuario_creacion"].ToString();
                        Order.fecha_creacion = sqlReader["fecha_creacion"].ToString();
                        Order.usuario_modifica = sqlReader["usuario_modifica"].ToString();
                        Order.fecha_modifica = sqlReader["fecha_modifica"].ToString();
                        Order.id = int.Parse(sqlReader["id"].ToString());
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
        public ForecastDetailUpdateBodyWeb SP_FORECAST_WEB_POST_DETAIL_UPDATE(ForecastDetailUpdateRequestWeb obj, string cnx)
        {
            ForecastDetailUpdateRequestWeb ordenC = new ForecastDetailUpdateRequestWeb();
            //ForecastDetailUpdateBodyWeb orderList = new ForecastDetailUpdateBodyWeb();
            var Order = new ForecastDetailUpdateBodyWeb();
            try
            {
                ordenC.id_almacen = obj.id_almacen;
                ordenC.forecast = obj.forecast;

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_FORECAST_DETAIL_UPDATE, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@forecast", SqlDbType.NVarChar).Value = obj.forecast;
                    cmd.Parameters.Add("@id_d_forecast", SqlDbType.Int).Value = obj.id_d_forecast;
                    cmd.Parameters.Add("@cantidad", SqlDbType.Float).Value = obj.cantidad;
                    cmd.Parameters.Add("@cantidad_recibir", SqlDbType.Float).Value = obj.cantidad_recibir;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = obj.usuario;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    
                    while (sqlReader.Read())
                    {
                        ForecastDetailUpdateDataWeb od = new ForecastDetailUpdateDataWeb();
                        Order.data = new List<ForecastDetailUpdateDataWeb>();
                        Order.tipo = int.Parse(sqlReader["tipo"].ToString());//0;
                        Order.mensaje = sqlReader["mensaje"].ToString();//"Registro correcto";
                        od.alerta = sqlReader["alerta"].ToString();
                        od.forecast = sqlReader["forecast"].ToString();
                        od.numero_item = sqlReader["numero_item"].ToString();
                        od.descripcion_item = sqlReader["descripcion_item"].ToString();
                        od.categoria_inventario = sqlReader["categoria_inventario"].ToString();
                        od.subcategoria_inventario = sqlReader["subcategoria_inventario"].ToString();
                        od.atributo_01 = sqlReader["atributo_01"].ToString();
                        od.cantidad = float.Parse(sqlReader["cantidad"].ToString());
                        od.cantidad_recibir = float.Parse(sqlReader["cantidad_recibir"].ToString());
                        od.usuario_creacion = sqlReader["usuario_creacion"].ToString();
                        od.fecha_creacion = sqlReader["fecha_creacion"].ToString();
                        od.usuario_modifica = sqlReader["usuario_modifica"].ToString();
                        od.fecha_modifica = sqlReader["fecha_modifica"].ToString();
                        od.id = int.Parse(sqlReader["id"].ToString());
                        Order.data.Add(od);
                    }

                    conn.Close();

                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            return Order;
        }
    }
}
