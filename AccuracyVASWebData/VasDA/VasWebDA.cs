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
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_PRODUCTION_LINE_BY_TERMINAL, conn))
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
    }
}

