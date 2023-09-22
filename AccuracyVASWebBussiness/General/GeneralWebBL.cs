using AccuracyData.General;
using AccuracyData.KPI;
using AccuracyModel.General;
using AccuracyModel.KPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyBussiness.General
{
    public class GeneralWebBL
    {
        public List<GeneralObjetotBodyWeb> SP_KPI_WEB_GET_LISTA_ATRIBUTO_OBJETO(GeneralObjetoRequestWeb model, string cnx)
        {
            GeneralWebDA poObjects = new GeneralWebDA();
            List<GeneralObjetotBodyWeb> resp = new List<GeneralObjetotBodyWeb>();
            resp = poObjects.SP_KPI_WEB_GET_LISTA_ATRIBUTO_OBJETO(model, cnx);
            return resp;
        }
        /*Envio de curvas ITSANET*/
        public List<CurvaRoot> SP_GENERAL_WEB_POST_SEND_CURVAS_BY_OC(CurvaRequest model, string cnx)
        {
            GeneralWebDA poObjects = new GeneralWebDA();
            List<CurvaRoot> resp = new List<CurvaRoot>();
            resp = poObjects.SP_GENERAL_WEB_POST_SEND_CURVAS_BY_OC(model, cnx);
            return resp;
        }
    }
}
