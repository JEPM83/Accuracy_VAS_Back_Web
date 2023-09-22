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
    }
}
