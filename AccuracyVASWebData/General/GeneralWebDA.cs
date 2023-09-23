using AccuracyData.ObjectosDA;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AccuracyModel.General;

namespace AccuracyData.General
{
    public class GeneralWebDA
    {
        public List<GeneralObjetotBodyWeb> SP_KPI_WEB_GET_LISTA_ATRIBUTO_OBJETO(GeneralObjetoRequestWeb obj, string cnx)
        {
            GeneralObjetoRequestWeb ordenC = new GeneralObjetoRequestWeb();
            List<GeneralObjetotBodyWeb> orderList = new List<GeneralObjetotBodyWeb>();
            try
            {
                ordenC.objeto = obj.objeto;
                ordenC.tipo = obj.tipo;
                ordenC.idioma = obj.idioma;
                ordenC.modo = obj.modo;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.GENERAL_GET_LISTA_ATRIBUTO_OBJETO, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@objeto", SqlDbType.VarChar).Value = obj.objeto;
                    cmd.Parameters.Add("@tipo", SqlDbType.NVarChar).Value = obj.tipo;
                    cmd.Parameters.Add("@idioma", SqlDbType.NVarChar).Value = obj.idioma;
                    cmd.Parameters.Add("@modo", SqlDbType.Bit).Value = obj.modo;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var Order = new GeneralObjetotBodyWeb();
                        Order.value = sqlReader["Value"].ToString();
                        Order.display = sqlReader["Display"].ToString();
                        Order.display_large = sqlReader["Display_Large"].ToString();
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
