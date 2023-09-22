using AccuracyData.KPI;
using AccuracyModel.KPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyBussiness.KPI
{
    public class OcupabilidadWebBL
    {
        public List<OcupabilidadBodyWeb> SP_KPI_WEB_GET_OCUPABILIDAD(OcupabilidadRequestWeb model, string HostGroupId, string cnx)
        {
            OcupabilidadWebDA poObjects = new OcupabilidadWebDA();
            List<OcupabilidadBodyWeb> resp = new List<OcupabilidadBodyWeb>();
            resp = poObjects.SP_KPI_WEB_GET_OCUPABILIDAD(model, cnx);
            return resp;
        }
    }
}
