using AccuracyData.PrinterDA;
using AccuracyModel.Printer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyBussiness.PrinterBL
{
    public class PrinterWebBL
    {
        public List<PrinterBodyWeb> SP_PRINTER_WEB_GET_CONFIG(PrinterRequestWeb model, string cnx)
        {
            PrinterWebDA poObjects = new PrinterWebDA();
            List<PrinterBodyWeb> resp = poObjects.SP_PRINTER_WEB_GET_CONFIG(model, cnx);
            return resp;
        }
        public List<BodyLPNWeb> SP_PRINTER_WEB_POST_INSERT_PRINTER_LPN(RequestLPNWeb model, string cnx)
        {
            PrinterWebDA poObjects = new PrinterWebDA();
            List<BodyLPNWeb> resp = poObjects.SP_PRINTER_WEB_POST_INSERT_PRINTER_LPN(model, cnx);
            return resp;
        }
        public BodyCorrelativoLPNWeb SP_PRINTER_WEB_GET_LPN_CORRELATIVE(RequestCorrelativoLPNWeb model, string cnx)
        {
            PrinterWebDA poObjects = new PrinterWebDA();
            BodyCorrelativoLPNWeb resp = poObjects.SP_PRINTER_WEB_GET_LPN_CORRELATIVE(model, cnx);
            return resp;
        }
        public BodyGenerabultos SP_PRINTER_WEB_POST_CONVERTED_PACK(RequestGenerabultos model, string cnx)
        {
            PrinterWebDA poObjects = new PrinterWebDA();
            BodyGenerabultos resp = poObjects.SP_PRINTER_WEB_POST_CONVERTED_PACK(model, cnx);
            return resp;
        }
        public List<BodyCabeceraBultos> SP_PRINTER_WEB_GET_PACK(RequestCabeceraBultos model, string cnx)
        {
            PrinterWebDA poObjects = new PrinterWebDA();
            List<BodyCabeceraBultos> resp = poObjects.SP_PRINTER_WEB_GET_PACK(model, cnx);
            return resp;
        }
        public List<BodyDetalleBultos> SP_PRINTER_WEB_GET_PACK_DETAIL(RequestDetalleBultos model, string cnx)
        {
            PrinterWebDA poObjects = new PrinterWebDA();
            List<BodyDetalleBultos> resp = poObjects.SP_PRINTER_WEB_GET_PACK_DETAIL(model, cnx);
            return resp;
        }
        public List<ResponseBultoxBulto> SP_PRINTER_WEB_POST_INSERT_PRINTER_BULTO(RequestBultoxBulto model, string cnx)
        {
            PrinterWebDA poObjects = new PrinterWebDA();
            List<ResponseBultoxBulto> resp = poObjects.SP_PRINTER_WEB_POST_INSERT_PRINTER_BULTO(model, cnx);
            return resp;
        }
    }
}
