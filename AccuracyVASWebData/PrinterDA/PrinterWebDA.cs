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
using AccuracyModel.Outbound;

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
        public BodyGenerabultos SP_PRINTER_WEB_POST_CONVERTED_PACK(RequestGenerabultos obj, string cnx)
        {
            RequestGenerabultos printerC = new RequestGenerabultos();
            var printer = new BodyGenerabultos();
            try
            {
                printerC.id_almacen = obj.id_almacen;
                printerC.id_printer = obj.id_printer;
                printerC.numero_orden_compra = obj.numero_orden_compra;
                printerC.usuario_creacion = obj.usuario_creacion;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_POST_CONVERTED_PACK, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.VarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@id_printer", SqlDbType.Int).Value = obj.id_printer;
                    cmd.Parameters.Add("@numero_orden_compra", SqlDbType.VarChar).Value = obj.numero_orden_compra;
                    cmd.Parameters.Add("@usuario_creacion", SqlDbType.VarChar).Value = obj.usuario_creacion;
                    cmd.Parameters.Add("@impresion", SqlDbType.Bit).Value = obj.impresion;
                    cmd.Parameters.Add("@documento", SqlDbType.NVarChar).Value = obj.documento;
                    cmd.Parameters.Add("@numero_item", SqlDbType.NVarChar).Value = obj.numero_item;
                    cmd.Parameters.Add("@cantidad_impresion", SqlDbType.Int).Value = obj.cantidad_impresion;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        
                        printer.title = "Aviso";
                        printer.message = sqlReader["MESSAGE"].ToString();
                        if (sqlReader["STATE"].ToString() == "0")
                        {
                            printer.type = 0;
                            printer.statuscode = 200;
                        }
                        else {
                            printer.type = 3;
                            printer.statuscode = 400;
                        }
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
        public List<BodyCabeceraBultos> SP_PRINTER_WEB_GET_PACK(RequestCabeceraBultos obj, string cnx)
        {
            RequestCabeceraBultos printerC = new RequestCabeceraBultos();
            List<BodyCabeceraBultos> printerList = new List<BodyCabeceraBultos>();
            try
            {
                printerC.id_almacen = obj.id_almacen;
                printerC.numero_orden_compra = obj.numero_orden_compra;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_PACK, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.VarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@numero_orden_compra", SqlDbType.VarChar).Value = obj.numero_orden_compra;
                    cmd.Parameters.Add("@documento", SqlDbType.VarChar).Value = obj.documento;
                    cmd.Parameters.Add("@numero_item", SqlDbType.VarChar).Value = obj.numero_item;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var printer = new BodyCabeceraBultos();
                        printer.id_bulto = int.Parse(sqlReader["id_bulto"].ToString());
                        printer.id_almacen= sqlReader["id_almacen"].ToString();
                        printer.numero_orden_compra = sqlReader["numero_orden_compra"].ToString();
                        printer.linea = sqlReader["linea"].ToString();
                        printer.numero_bulto = sqlReader["numero_bulto"].ToString();
                        printer.cantidad = float.Parse(sqlReader["cantidad"].ToString());
                        printer.cartones = float.Parse(sqlReader["cartones"].ToString());
                        printer.destino = sqlReader["destino"].ToString();
                        printer.nota = sqlReader["nota"].ToString();
                        printer.curva = sqlReader["curva"].ToString();
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
        public List<BodyDetalleBultos> SP_PRINTER_WEB_GET_PACK_DETAIL(RequestDetalleBultos obj, string cnx)
        {
            RequestDetalleBultos printerC = new RequestDetalleBultos();
            List<BodyDetalleBultos> printerList = new List<BodyDetalleBultos>();
            try
            {
                printerC.id_almacen = obj.id_almacen;
                printerC.id_bulto = obj.id_bulto;
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_GET_PACK_DETAIL, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.VarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@id_bulto", SqlDbType.Int).Value = obj.id_bulto;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var printer = new BodyDetalleBultos();
                        printer.id_bulto_detalle = int.Parse(sqlReader["id_bulto_detalle"].ToString());
                        printer.id_bulto = int.Parse(sqlReader["id_bulto"].ToString());
                        printer.id_almacen = sqlReader["id_almacen"].ToString();
                        printer.numero_orden_compra = sqlReader["numero_orden_compra"].ToString();
                        printer.numero_item = sqlReader["numero_item"].ToString();
                        printer.ean13 = sqlReader["ean13"].ToString();
                        printer.cantidad = float.Parse(sqlReader["cantidad"].ToString());
                        printer.color = sqlReader["color"].ToString();
                        printer.talla = sqlReader["talla"].ToString();
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
        public List<ResponseBultoxBulto> SP_PRINTER_WEB_POST_INSERT_PRINTER_BULTO(RequestBultoxBulto obj, string cnx)
        {
            List<ResponseBultoxBulto> printerList = new List<ResponseBultoxBulto>();
            try
            {
                using (SqlConnection conn = new SqlConnection(cnx))
                using (SqlCommand cmd = new SqlCommand(ObjectsDA.WEB_POST_INSERT_PRINTER_BULTO, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id_almacen", SqlDbType.NVarChar).Value = obj.id_almacen;
                    cmd.Parameters.Add("@id_printer", SqlDbType.Int).Value = obj.id_printer;
                    cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = obj.cantidad;
                    cmd.Parameters.Add("@numero_item", SqlDbType.NVarChar).Value = obj.numero_item;
                    cmd.Parameters.Add("@talla", SqlDbType.NVarChar).Value = obj.talla;
                    cmd.Parameters.Add("@ean", SqlDbType.NVarChar).Value = obj.ean;
                    cmd.Parameters.Add("@cantidad_bulto", SqlDbType.Int).Value = obj.cantidad_bulto;
                    cmd.Parameters.Add("@importacion", SqlDbType.NVarChar).Value = obj.importacion;
                    cmd.Parameters.Add("@numero_orden_compra", SqlDbType.NVarChar).Value = obj.numero_orden_compra;
                    cmd.Parameters.Add("@destino", SqlDbType.NVarChar).Value = obj.destino;
                    cmd.Parameters.Add("@usuario_creacion", SqlDbType.NVarChar).Value = obj.usuario_creacion;
                    conn.Open();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        var printer = new ResponseBultoxBulto();
                        printer.title = "Aviso";
                        printer.message = sqlReader["MESSAGE"].ToString();
                        printer.type = 0;
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
        }
    }
}
