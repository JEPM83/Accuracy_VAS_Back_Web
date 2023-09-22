using AccuracyData.SecurityDA;
using AccuracyData.VasDA;
using AccuracyModel.Security;
using AccuracyModel.Vas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyBussiness.VasBL
{
    public class VasWebBL
    {
        public List<LineResponse> GET_PRODUCTION_LINE_BY_TERMINAL(LineRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<LineResponse> resp = new List<LineResponse>();
            resp = poObjects.GET_PRODUCTION_LINE_BY_TERMINAL(model, cnx);
            return resp;
        }
    }
}
