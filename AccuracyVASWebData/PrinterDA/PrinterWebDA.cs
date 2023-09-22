using AccuracyData.ObjectosDA;
using AccuracyModel.Printer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace AccuracyData.PrinterDA
{
    public class PrinterWebDA
    {
        public List<PrinterBodyWeb> SP_PRINTER_WEB_GET_CONFIG(PrinterRequestWeb obj, string cnx)
        {
            PrinterRequestWeb printerC = new PrinterRequestWeb();
            List<PrinterBodyWeb> printerList = new List<PrinterBodyWeb>();
            try
            {
                printerC.id_almacen = obj.id_almacen;
                printerC.id_printer = obj.id_printer;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_PRINTER_CONFIG, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FacilityCode", SqlDbType.VarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@PrintID", SqlDbType.VarChar).Value = obj.id_printer;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var printer = new PrinterBodyWeb();
                        printer.id_printer = sqlReader["id_printer"].ToString();
                        printer.host_name = sqlReader["host_name"].ToString();
                        printer.host_ip = sqlReader["host_ip"].ToString();
                        printer.lan_url = sqlReader["lan_url"].ToString();
                        printer.extension_file = sqlReader["extension_file"].ToString();
                        printer.state = sqlReader["state"].ToString();
                        printer.id_almacen = sqlReader["id_almacen"].ToString();
                        printerList.Add(printer);
                    }
                    conn.Close();
                    return printerList;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                printerC = null;
            }
        }
        public List<BodyLPNWeb> SP_PRINTER_WEB_POST_INSERT_PRINTER_LPN(RequestLPNWeb obj, string cnx)
        {
            RequestLPNWeb printerC = new RequestLPNWeb();
            List<BodyLPNWeb> printerList = new List<BodyLPNWeb>();
            try
            {
                printerC.id_almacen = obj.id_almacen;
                printerC.id_printer = obj.id_printer;
                printerC.cantidad = obj.cantidad;
                printerC.usuario_creacion = obj.usuario_creacion;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_POST_INSERT_PRINTER_LPN, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.VarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@id_printer", SqlDbType.VarChar).Value = obj.id_printer;
                    cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = obj.cantidad;
                    cmd.Parameters.Add("@usuario_creacion", SqlDbType.VarChar).Value = obj.usuario_creacion;  
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var printer = new BodyLPNWeb();
                        printer.title = "Aviso";
                        printer.message = sqlReader["MESSAGE"].ToString();
                        printer.type = "0";
                        printer.statuscode = 200;
                        printerList.Add(printer);
                    }
                    conn.Close();
                    return printerList;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                printerC = null;
            }
        }
        public BodyCorrelativoLPNWeb SP_PRINTER_WEB_GET_LPN_CORRELATIVE(RequestCorrelativoLPNWeb obj, string cnx)
        {
            RequestLPNWeb printerC = new RequestLPNWeb();
            var printer = new BodyCorrelativoLPNWeb();
            try
            {
                printerC.id_almacen = obj.id_almacen;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_LPN_CORRELATIVE, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.VarChar).Value = obj.id_almacen;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        printer.lpn = sqlReader["ultimo_lpn"].ToString();
                    }
                    conn.Close();
                    return printer;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                printerC = null;
            }
        }
    }
}
