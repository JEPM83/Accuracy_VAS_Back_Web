using AccuracyData.InboundDA;
using AccuracyData.LoadDA;
using AccuracyModel.Inbound;
using AccuracyModel.Load;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyBussiness.LoadBL
{
    public class LoadImportWebBL
    {
        public bool SP_LOAD_WEB_GET_ORDER_IMPORT(LoadImportRequest model, string xml, string cnx)
        {
            LoadImportWebDA poObjects = new LoadImportWebDA();
            bool resp = poObjects.SP_LOAD_WEB_GET_ORDER_IMPORT(model, xml, cnx);
            return resp;
        }
    }
}
