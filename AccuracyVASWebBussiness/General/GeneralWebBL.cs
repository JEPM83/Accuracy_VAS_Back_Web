using AccuracyData.General;
using AccuracyModel.General;
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
    }
}
