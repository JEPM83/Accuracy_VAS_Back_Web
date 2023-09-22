using AccuracyData.ForecastDA;
using AccuracyModel.Forecast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyBussiness.ForecastBL
{
    public class ForecastWebBL
    {
        public List<ForecasHeadtBodyWeb> SP_FORECAST_WEB_GET_HEAD(ForecastHeadRequestWeb model, string HostGroupId, string cnx)
        {
            ForecastWebDA poObjects = new ForecastWebDA();
            List<ForecasHeadtBodyWeb> resp = new List<ForecasHeadtBodyWeb>();
            resp = poObjects.SP_FORECAST_WEB_GET_HEAD(model, cnx);
            return resp;
        }
        public List<ForecastDetailBodyWeb> SP_FORECAST_WEB_GET_DETAIL(ForecastDetailRequestWeb model, string HostGroupId, string cnx)
        {
            ForecastWebDA poObjects = new ForecastWebDA();
            List<ForecastDetailBodyWeb> resp = new List<ForecastDetailBodyWeb>();
            resp = poObjects.SP_FORECAST_WEB_GET_DETAIL(model, cnx);
            return resp;
        }
        public ForecastDetailUpdateBodyWeb SP_FORECAST_WEB_GET_DETAIL_UPDATE(ForecastDetailUpdateRequestWeb model, string HostGroupId, string cnx)
        {
            ForecastWebDA poObjects = new ForecastWebDA();
            ForecastDetailUpdateBodyWeb resp = new ForecastDetailUpdateBodyWeb();
            resp = poObjects.SP_FORECAST_WEB_POST_DETAIL_UPDATE(model, cnx);
            return resp;
        }
    }
}
