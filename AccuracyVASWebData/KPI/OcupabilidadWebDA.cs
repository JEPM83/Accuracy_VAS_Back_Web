using AccuracyData.ObjectosDA;
using AccuracyModel.KPI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AccuracyModel.Outbound;

namespace AccuracyData.KPI
{
    public class OcupabilidadWebDA
    {
        public List<OcupabilidadBodyWeb> SP_KPI_WEB_GET_OCUPABILIDAD(OcupabilidadRequestWeb obj, string cnx)
        {
            OcupabilidadRequestWeb ordenC = new OcupabilidadRequestWeb();
            List<OcupabilidadBodyWeb> orderList = new List<OcupabilidadBodyWeb>();
            try
            {
                ordenC.id_almacen = obj.id_almacen;
                ordenC.fecha_inicial = obj.fecha_inicial;
                ordenC.fecha_final = obj.fecha_final;
                ordenC.area = obj.area;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_KPI_OCUPABILIDAD, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.VarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@fecha_inicial", SqlDbType.NVarChar).Value = obj.fecha_inicial;
                    cmd.Parameters.Add("@fecha_final", SqlDbType.NVarChar).Value = obj.fecha_final;
                    cmd.Parameters.Add("@area", SqlDbType.Int).Value = obj.area;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var Order = new OcupabilidadBodyWeb();
                        Order.unidad = sqlReader["unidad"].ToString(); ;
                        Order.total = Decimal.Parse(sqlReader["total"].ToString());
                        Order.ocupado = Decimal.Parse(sqlReader["ocupado"].ToString());
                        Order.area = sqlReader["area"].ToString();
                        Order.fecha = sqlReader["fecha"].ToString();
                        Order.actual = int.Parse(sqlReader["actual"].ToString());
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
    }
}
