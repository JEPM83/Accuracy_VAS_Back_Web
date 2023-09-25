using AccuracyData.ObjectosDA;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccuracyModel.Security;

namespace AccuracyData.SecurityDA
{
    public class SecurityWebDA
    {
        public List<UserResponse> Login(UserRequest model,string cnx)
        {
            var plListDetail = new List<UserResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.SECURITY_GET_DATALISTLOGIN, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = model.usuario;
                    cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = model.password;
                    cmd.Parameters.Add("@sistema", SqlDbType.NVarChar).Value = model.sistema;
                    cmd.Parameters.Add("@identificador", SqlDbType.NVarChar).Value = model.identificador;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new UserResponse();
                        plDetail.usuario = sqlReader["usuario"].ToString();
                        plDetail.estado_sesion = sqlReader["estado_sesion"].ToString();
                        plDetail.guid_sesion = sqlReader["guid_sesion"].ToString();
                        plDetail.status = sqlReader["status"].ToString();
                        plDetail.mensaje = sqlReader["mensaje"].ToString();
                        plDetail.linea_produccion = sqlReader["linea_produccion"].ToString();
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
        public List<UserResponse> Logout(UserRequest model,string cnx)
        {
            var plListDetail = new List<UserResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.SECURITY_GET_DATALISTLOGOUT, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = model.usuario;
                    cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = model.password;
                    cmd.Parameters.Add("@sistema", SqlDbType.NVarChar).Value = model.sistema;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new UserResponse();
                        plDetail.usuario = sqlReader["usuario"].ToString();
                        plDetail.estado_sesion = sqlReader["estado_sesion"].ToString();
                        plDetail.guid_sesion = sqlReader["guid_sesion"].ToString();
                        plDetail.status = sqlReader["status"].ToString();
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
        public List<UserWarehouseResponse> WarehouseUser(UserWarehouseRequest model, string cnx)
        {
            var plListDetail = new List<UserWarehouseResponse>();
            try
            {

                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.SECURITY_GET_WAREHOUSE_USER, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usuario", SqlDbType.NVarChar).Value = model.usuario;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var plDetail = new UserWarehouseResponse();
                        plDetail.id_almacen = sqlReader["id_almacen"].ToString();
                        plDetail.codigo = sqlReader["codigo"].ToString();
                        plDetail.nombre = sqlReader["nombre"].ToString();
                        plDetail.codigo_pais = sqlReader["codigo_pais"].ToString();
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

