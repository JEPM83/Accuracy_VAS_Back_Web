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
using AccuracyModel.Load;

namespace AccuracyData.LoadDA
{
    public class LoadImportWebDA
    {
        public bool SP_LOAD_WEB_GET_ORDER_IMPORT(LoadImportRequest model, string xml, string cnx)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_LOAD_OBJECT_IMPORT, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.VarChar).Value = model.id_almacen.ToString();
                    cmd.Parameters.Add("@xml", SqlDbType.Xml).Value = xml.ToString();
                    cmd.Parameters.Add("@gui", SqlDbType.VarChar).Value = model.gui.ToString();
                    cmd.Parameters.Add("@user", SqlDbType.VarChar).Value = model.user.ToString();
                    cmd.Parameters.Add("@proceso", SqlDbType.VarChar).Value = model.proceso.ToString();
                    cmd.Parameters.Add("@cliente", SqlDbType.VarChar).Value = model.cliente.ToString();
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    //while (sqlReader.Read())
                    //{
                    //    var Order = new PurchaseOrderBodyWeb();
                    //    Order.numero_orden_compra = sqlReader["NUMERO_ORDEN_COMPRA"].ToString(); ;
                    //    Order.tipo_orden = sqlReader["TIPO_ORDEN"].ToString();
                    //    Order.estado = sqlReader["ESTADO"].ToString();
                    //    Order.nombre_proveedor = sqlReader["NOMBRE_PROVEEDOR"].ToString();
                    //    Order.almacen = sqlReader["ALMACEN"].ToString();
                    //    Order.fecha_entrega_estimada = sqlReader["FECHA_ENTREGA_ESTIMADA"].ToString();
                    //    Order.lineas = sqlReader["LINEAS"].ToString();
                    //    orderList.Add(Order);
                    //}
                    conn.Close();
                    return true;

                }
                
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                //ordenC = null;
            }

        }
    }
}
