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
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_LISTA_ATRIBUTO_OBJETO, conn))
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
        /*Curva ITSANET*/
        public List<CurvaRoot> SP_GENERAL_WEB_POST_SEND_CURVAS_BY_OC(CurvaRequest obj, string cnx)
        {
            List<CurvaRoot> orderList = new List<CurvaRoot>();
            try
            {
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_POST_SEND_CURVAS_BY_OC, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.VarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@numero_orden_compra", SqlDbType.NVarChar).Value = obj.numero_orden_compra;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    int contador = 1;
                    string codigo_master = string.Empty;
                    var Curva = new CurvaRoot();
                    var ListaCurva = new CurvaDetalle();
                    List<CurvaDetalle> detalle = new List<CurvaDetalle>();
                    while (sqlReader.Read())
                    {
                        if (contador == 1)
                        {
                            codigo_master = sqlReader["CODIGO_MASTER"].ToString();
                            Curva.COD_EMPRESA = sqlReader["COD. EMPRESA"].ToString();
                            //Curva.COD_EMPRESA = sqlReader["CODIGO_MASTER"].ToString();
                            Curva.NRO_PACKING = sqlReader["NRO. PACKING"].ToString();
                            Curva.CANTIDAD_BULTOS = int.Parse(sqlReader["CANTIDAD_BULTOS"].ToString());
                            Curva.ORDEN_DE_COMPRA = sqlReader["ORDEN DE COMPRA"].ToString();
                            Curva.ERROR = sqlReader["ERROR"].ToString();
                        }
                        if (codigo_master == sqlReader["CODIGO_MASTER"].ToString())
                        {
                            ListaCurva = new CurvaDetalle();
                            ListaCurva.EAN = sqlReader["EAN"].ToString();
                            ListaCurva.CANTIDAD = int.Parse(sqlReader["CANTIDAD"].ToString());
                            ListaCurva.ERROR = sqlReader["ERROR_D"].ToString();
                            ListaCurva.MESSAGE = sqlReader["MESSAGE"].ToString();
                            detalle.Add(ListaCurva);
                        }
                        else {
                            Curva = new CurvaRoot();
                            ListaCurva = new CurvaDetalle();
                            detalle = new List<CurvaDetalle>();
                            codigo_master = sqlReader["CODIGO_MASTER"].ToString();
                            Curva.COD_EMPRESA = sqlReader["COD. EMPRESA"].ToString();
                            //Curva.COD_EMPRESA = sqlReader["CODIGO_MASTER"].ToString();
                            Curva.NRO_PACKING = sqlReader["NRO. PACKING"].ToString();
                            Curva.CANTIDAD_BULTOS = int.Parse(sqlReader["CANTIDAD_BULTOS"].ToString());
                            Curva.ORDEN_DE_COMPRA = sqlReader["ORDEN DE COMPRA"].ToString();
                            Curva.ERROR = sqlReader["ERROR"].ToString();
                            ListaCurva.EAN = sqlReader["EAN"].ToString();
                            ListaCurva.CANTIDAD = int.Parse(sqlReader["CANTIDAD"].ToString());
                            ListaCurva.ERROR = sqlReader["ERROR_D"].ToString();
                            ListaCurva.MESSAGE = sqlReader["MESSAGE"].ToString();
                            detalle.Add(ListaCurva);
                        }
                        Curva.DETALLE = detalle;
                        orderList.Add(Curva);
                        contador++;
                    }
                    conn.Close();
                    return orderList;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}
